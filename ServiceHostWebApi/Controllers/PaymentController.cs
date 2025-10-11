using MasterManagement.Domain.PaymentMethodsTypeAgg;
using MasterManagment.Application.Contracts.OrderContracts;
using MasterManagment.Application.Contracts.OrderItem;
using MasterManagment.Application.Contracts.Payment;
using MasterManagment.Application.Contracts.Shipping;
using Microsoft.AspNetCore.Mvc;

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

    public PaymentController(IPaymentApplication paymentApplication, ICartApplication cartApplication, IOrderApplication orderApplication , IShippingApplication shippingApplication )
    {
        _paymentApplication = paymentApplication;
        _cartApplication = cartApplication;
        _orderApplication = orderApplication;
        _shippingApplication = shippingApplication;
    }


    [HttpPost("create")]
    [ProducesResponseType(200, Type = typeof(long))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var paymentId = await _paymentApplication.CreateAsync(command);
            return Ok(new { PaymentId = paymentId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
    }

   
    [HttpPut("edit")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Edit([FromBody] EditPaymentCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _paymentApplication.UpdateAsync(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

  
    [HttpPut("cancel/{id:long}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Cancel(long id)
    {
        try
        {
            await _paymentApplication.CancelAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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

}

   


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