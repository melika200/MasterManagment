using System.Security.Claims;
using System.Text;
using AccountManagment.Contracts.UserContracts;
using MasterManagement.Domain.PaymentMethodsTypeAgg;
using MasterManagment.Application.Contracts.OrderContracts;
using MasterManagment.Application.Contracts.OrderItem;
using MasterManagment.Application.Contracts.Payment;
using MasterManagment.Application.Contracts.Shipping;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ServiceHostWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentApplication _paymentApplication;
    private readonly ICartApplication _cartApplication;
    private readonly IOrderApplication _orderApplication;
    private readonly IShippingApplication _shippingApplication;
    private readonly IUserApplication _userApplication;

    public PaymentController(IPaymentApplication paymentApplication, ICartApplication cartApplication, IOrderApplication orderApplication, IShippingApplication shippingApplication, IUserApplication userApplication)
    {
        _paymentApplication = paymentApplication;
        _cartApplication = cartApplication;
        _orderApplication = orderApplication;
        _shippingApplication = shippingApplication;
        _userApplication = userApplication;
    }

    [HttpGet("paymentMethods")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetPaymentMethodsAsync()
    {
        var methods = await Task.FromResult(
            PaymentMethodsType.AllMethods
                .Select(m => new
                {
                    m.Id,
                    m.Name
                }).ToList());

        return Ok(methods);
    }
    [HttpGet("{id:long}")]
    [ProducesResponseType(200, Type = typeof(PaymentViewModel))]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var payment = await _paymentApplication.GetByIdAsync(id);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    ///  // دریافت لیست پرداخت‌ها بر اساس شناسه کارت
    /// </summary>
    /// <param name="cartId"></param>
    /// <returns></returns>
    [HttpGet("cart/{cartId:long}")]
    [ProducesResponseType(200, Type = typeof(List<PaymentViewModel>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetByCartId(long cartId)
    {
        try
        {
            var payments = await _paymentApplication.GetByCartIdAsync(cartId);
            return Ok(payments);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

  

    [HttpPost("confirm-payment")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _paymentApplication.ConfirmAsync(command);
            if (!result.IsSuccedded)
                return BadRequest(new { message = result.Message });

            var cartItems = await _cartApplication.GetItemsAsync(command.CartId);
            var cartAmount = await _cartApplication.GetAmountByAsync(command.CartId);
            var shipping = await _shippingApplication.GetByCartIdAsync(command.CartId);

            var orderItems = cartItems.Select(item => new OrderItemViewModel
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Count=item.Count,
                UnitPrice=item.UnitPrice
               
            }).ToList();

            var createOrderCommand = new CreateOrderCommand
            {
                AccountId = command.AccountId,
                CartId = command.CartId,
                PaymentId = command.PaymentId,
                PaymentMethodId = command.PaymentMethodId,
                PaymentMethodName=command.PaymentMethodName,
                DiscountAmount = command.DiscountAmount,
                PayAmount = command.PayAmount,
                TotalAmount = cartAmount,
                Items = orderItems,
                ShippingInfo = shipping
            };

            var orderId = await _orderApplication.CreateAsync(createOrderCommand);

            await _cartApplication.DeleteAsync(command.CartId);

            return Ok(new { OrderId = orderId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //[Authorize]
    [HttpPost("start-zarinpal")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> StartZarinpalPayment([FromBody] long cartId)
    {
        var cartAmount = await _cartApplication.GetAmountByAsync(cartId);
        var accountId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var user = (await _userApplication.GetAccountsByIds(new List<long> { accountId })).FirstOrDefault();
        if (user == null)
            return BadRequest("کاربر یافت نشد");

        var request = new
        {
            merchant_id = "یادم باشه برم ثبت نام کنم تو زرین پال بعد اینجا پر کنم ",
            amount = (int)cartAmount,
            description = "پرداخت برای سفارش",
            callback_url = $"https://yourdomain.com/api/v1/payment/verify-zarinpal?cartId={cartId}",
            metadata = new
            {
                mobile = user.PhoneNumber,
                //email = user.Email,
                order_id = cartId.ToString()
            }
        };

        var httpClient = new HttpClient();
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("https://payment.zarinpal.com/pg/v4/payment/request.json", content);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ZarinPalRequestResponse>(responseString);

        if (result?.data?.code == 100)
        {
            var authority = result.data.authority;
            var redirectUrl = $"https://payment.zarinpal.com/pg/StartPay/{authority}";
            return Ok(new { redirectUrl });
        }

        return BadRequest("خطا در ارتباط با درگاه پرداخت: " + result?.data?.message);
    }


    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [HttpGet("verify-zarinpal")]
    public async Task<IActionResult> VerifyZarinpalPayment([FromQuery] string authority, [FromQuery] string status, [FromQuery] long cartId)
    {
        if (string.IsNullOrEmpty(authority) || status.ToLower() != "ok")
            return BadRequest("پرداخت توسط کاربر لغو شده یا ناموفق بوده است.");

        var cartAmount = await _cartApplication.GetAmountByAsync(cartId);
        var cart = await _cartApplication.GetCartByIdAsync(cartId);
        if (cart == null)
            return BadRequest("سبد خرید یافت نشد");
        var paymentMethodId = cart.PaymentMethodId;
        var paymentMethodName = cart.PaymentMethodName;
        var discountAmount = cart.DiscountAmount;
        var accountId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
       

        var verifyRequest = new ZarinPalVerifyRequest
        {
            merchant_id = "یادم باشه برم ثبت نام کنم تو زرین پال بعد اینجا پر کنم ",
            authority = authority,
            amount = (int)cartAmount
        };

        var httpClient = new HttpClient();
        var json = JsonConvert.SerializeObject(verifyRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("https://payment.zarinpal.com/pg/v4/payment/verify.json", content);
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ZarinPalVerifyResponse>(responseString);
        var payment = await _paymentApplication.GetByTransactionIdAsync(result.data.ref_id.ToString());
        if (payment == null)
            return BadRequest("پرداخت ثبت شده یافت نشد");

        if (result?.data?.code == 100)
        {
            var paymentCommand = new CreatePaymentCommand
            {
                CartId = cartId,
                Amount = cartAmount,
                TransactionId = result.data.ref_id.ToString(),
                IsSucceeded = true
            };

            var paymentResult = await _paymentApplication.CreateAsync(paymentCommand);
            if (!paymentResult.IsSuccedded)
                return BadRequest(paymentResult.Message);

            var confirmCommand = new ConfirmPaymentCommand
            {
                CartId = cartId,
                PaymentId = payment.Id,
                AccountId = accountId,
                PaymentMethodId = paymentMethodId,
                PaymentMethodName = paymentMethodName,
                DiscountAmount =discountAmount ,
                PayAmount = cartAmount
            };

            var confirmResult = await ConfirmPayment(confirmCommand);
            return Ok(new { message = "پرداخت با موفقیت تأیید شد" });
        }

        return BadRequest("پرداخت ناموفق بود: " + result?.data?.message);
    }

}

    //[HttpPost("create")]
    //[ProducesResponseType(200, Type = typeof(long))]
    //[ProducesResponseType(400)]
    //public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    try
    //    {
    //        var paymentId = await _paymentApplication.CreateAsync(command);
    //        return Ok(new { PaymentId = paymentId });
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.InnerException?.Message ?? ex.Message);
    //    }
    //}

   
    //[HttpPut("edit")]
    //[ProducesResponseType(204)]
    //[ProducesResponseType(400)]
    //public async Task<IActionResult> Edit([FromBody] EditPaymentCommand command)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    try
    //    {
    //        await _paymentApplication.UpdateAsync(command);
    //        return NoContent();
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

  
    //[HttpPut("cancel/{id:long}")]
    //[ProducesResponseType(204)]
    //[ProducesResponseType(400)]
    //public async Task<IActionResult> Cancel(long id)
    //{
    //    try
    //    {
    //        await _paymentApplication.CancelAsync(id);
    //        return NoContent();
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}



//[HttpPost("search")]
//[ProducesResponseType(200, Type = typeof(List<PaymentViewModel>))]
//[ProducesResponseType(400)]
//public async Task<IActionResult> Search([FromBody] PaymentSearchCriteria criteria)
//{
//    try
//    {
//        var payments = await _paymentApplication.SearchAsync(criteria);
//        return Ok(payments);
//    }
//    catch (Exception ex)
//    {
//        return BadRequest(ex.Message);
//    }
//}

   //[HttpGet("verify-zarinpal")]
   // [ProducesResponseType(200)]
   // [ProducesResponseType(400)]
   // public async Task<IActionResult> VerifyZarinpalAsync([FromQuery] string authority, [FromQuery] string status, [FromQuery] long cartId)
   // {
   //     if (string.IsNullOrEmpty(authority) || status.ToLower() != "ok")
   //         return BadRequest("پرداخت توسط کاربر لغو شده یا ناموفق بوده است.");

   //     var cartAmount = await _cartApplication.GetAmountByAsync(cartId);

   //     var verifyRequest = new
   //     {
   //         merchant_id = "YOUR_MERCHANT_ID",
   //         authority = authority,
   //         amount = (int)cartAmount
   //     };

   //     var httpClient = new HttpClient();
   //     var json = JsonConvert.SerializeObject(verifyRequest);
   //     var content = new StringContent(json, Encoding.UTF8, "application/json");

   //     var response = await httpClient.PostAsync("https://payment.zarinpal.com/pg/v4/payment/verify.json", content);
   //     var responseString = await response.Content.ReadAsStringAsync();
   //     var result = JsonConvert.DeserializeObject<ZarinPalV4VerifyResponse>(responseString);

   //     if (result?.data?.code == 100)
   //     {
   //         // ثبت پرداخت موفق
   //         var createPaymentCommand = new CreatePaymentCommand
   //         {
   //             CartId = cartId,
   //             Amount = cartAmount,
   //             TransactionId = result.data.ref_id.ToString(),
   //             IsSucceeded = true
   //         };

   //         var paymentResult = await _paymentApplication.CreateAsync(createPaymentCommand);
   //         if (!paymentResult.IsSuccedded)
   //             return BadRequest(paymentResult.Message);

   //         // گرفتن اطلاعات تکمیلی برای ساخت سفارش
   //         var cartItems = await _cartApplication.GetItemsAsync(cartId);
   //         var shipping = await _shippingApplication.GetByCartIdAsync(cartId);
   //         var accountId = /* از توکن یا سشن بگیر */ 1;

   //         var orderItems = cartItems.Select(item => new OrderItemViewModel
   //         {
   //             ProductId = item.ProductId,
   //             ProductName = item.ProductName,
   //             Count = item.Count,
   //             UnitPrice = item.UnitPrice
   //         }).ToList();

   //         var createOrderCommand = new CreateOrderCommand
   //         {
   //             AccountId = accountId,
   //             CartId = cartId,
   //             PaymentId = 0, // اگر خواستی از دیتابیس بگیری
   //             PaymentMethodId = 1,
   //             PaymentMethodName = "Online",
   //             DiscountAmount = 0,
   //             PayAmount = cartAmount,
   //             TotalAmount = cartAmount,
   //             Items = orderItems,
   //             ShippingInfo = shipping
   //         };

   //         var orderId = await _orderApplication.CreateAsync(createOrderCommand);
   //         await _cartApplication.DeleteAsync(cartId);

   //         return Ok(new
   //         {
   //             Message = "پرداخت با موفقیت تأیید شد",
   //             RefId = result.data.ref_id,
   //             OrderId = orderId
   //         });
   //     }

   //     return BadRequest("پرداخت ناموفق بود: " + result?.data?.message);
   // }
