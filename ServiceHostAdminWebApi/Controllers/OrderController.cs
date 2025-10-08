using AccountManagment.Contracts.UserContracts;
using MasterManagment.Application.Contracts.OrderContracts;
using MasterManagment.Application.Contracts.OrderItem;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostAdminWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("admin/api/v{version:apiVersion}/[controller]")]
//[Authorize(Roles = "Admin,Programmer")]
public class OrderController : ControllerBase
{
    private readonly IOrderApplication _orderApplication;
    private readonly IUserApplication _userApplication;

    public OrderController(IOrderApplication orderApplication, IUserApplication userApplication)
    {
        _orderApplication = orderApplication;
        _userApplication = userApplication;
    }



    //[HttpGet("AllOrders")]
    //[ProducesResponseType(200, Type = typeof(List<OrderViewModel>))]
    //[ProducesResponseType(400)]
    //public async Task<IActionResult> GetAllOrders()
    //{
    //    try
    //    {

    //        var orders = await _orderApplication.GetOrders();

    //        //var userIds = orders.Select(o => o.AccountId).Distinct().ToList();

    //        //var users = await _userApplication.GetAccountsByIds(userIds);

    //        var result = orders.Select(order =>
    //        {
    //            //var user = users.FirstOrDefault(u => u.Id == order.AccountId);

    //            return new OrderViewModel
    //            {
    //                Id = order.Id,
    //                AccountId = order.AccountId,
    //                FullName = order.FullName ?? string.Empty,
    //                Mobile = order?.Mobile ?? string.Empty,
    //                Address = order?.Address ?? string.Empty,
    //                PaymentMethod = order.PaymentMethod,
    //                TotalAmount = order.TotalAmount,
    //                IsPaid = order.IsPaid,
    //                IsCanceled = order.IsCanceled,
    //                IssueTrackingNo = order.IssueTrackingNo

    //            };
    //        }).ToList();

    //        return Ok(result);
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}
    [HttpGet("AllOrders")]
    [ProducesResponseType(200, Type = typeof(List<OrderViewModel>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            var orders = await _orderApplication.GetOrders();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return BadRequest($"خطا در دریافت سفارش‌ها: {ex.Message}");
        }
    }



    

    [HttpGet("{id:long}")]
    [ProducesResponseType(200, Type = typeof(OrderDetailViewModel))]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetOrderById(long id)
    {
        try
        {
            var orderDetail = await _orderApplication.GetOrderDetailAsync(id);
            if (orderDetail == null)
                return NotFound($"سفارشی با شناسه {id} یافت نشد.");

            return Ok(orderDetail);
        }
        catch (Exception ex)
        {
            return BadRequest($"خطا در دریافت جزئیات سفارش: {ex.Message}");
        }
    }

  



    [HttpPost("{orderId}/setTrackingNumber")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> SetTrackingNumber(long orderId, [FromBody] string trackingNumber)
    {
        var result = await _orderApplication.SetTrackingNumberAsync(orderId, trackingNumber);

        if (!result.IsSuccedded)
            return result.Message == "سفارش یافت نشد" ? NotFound(result.Message) : BadRequest(result.Message);

        return Ok(new { Message = "کد رهگیری با موفقیت ثبت شد." });
    }


    [HttpPost("{orderId}/setState")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> SetState(long orderId, [FromBody] int stateId)
    {
        var result = await _orderApplication.SetOrderStateAsync(orderId, stateId);
        if (!result.IsSuccedded)
            return BadRequest(result.Message);

        return Ok();
    }

    [HttpPost("{orderId}/setShippingState")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> SetShippingState(long orderId, [FromBody] int shippingStateId)
    {
        var result = await _orderApplication.SetOrderShippingStateAsync(orderId, shippingStateId);
        if (!result.IsSuccedded)
            return BadRequest(result.Message);

        return Ok();
    }








    //[HttpPost("createFromCart")]
    //[ProducesResponseType(200, Type = typeof(long))]
    //[ProducesResponseType(400)]
    //public async Task<IActionResult> CreateFromCart([FromBody] CreateOrderFromCartCommand command)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    try
    //    {
    //        var orderId = await _orderApplication.FinalizeFromCartAsync(command.CartId, command.TransactionId!);
    //        return Ok(new { OrderId = orderId });
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    //[HttpPut("edit")]
    //[ProducesResponseType(204)]
    //[ProducesResponseType(400)]
    //public async Task<IActionResult> Edit([FromBody] EditOrderCommand command)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    try
    //    {
    //        await _orderApplication.EditAsync(command);
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
    //        await _orderApplication.CancelAsync(id);
    //        return NoContent();
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    //[HttpGet("{id:long}/amount")]
    //[ProducesResponseType(200, Type = typeof(decimal))]
    //[ProducesResponseType(400)]
    //public async Task<IActionResult> GetAmount(long id)
    //{
    //    try
    //    {
    //        var amount = await _orderApplication.GetAmountByAsync(id);
    //        return Ok(amount);
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}
    //[HttpPost("search")]
    //[ProducesResponseType(200, Type = typeof(List<OrderViewModel>))]
    //[ProducesResponseType(400)]
    //public async Task<IActionResult> Search([FromBody] OrderSearchCriteria criteria)
    //{
    //    try
    //    {
    //        var orders = await _orderApplication.SearchAsync(criteria);
    //        return Ok(orders);
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    //[HttpGet("{id:long}/items")]
    //[ProducesResponseType(200, Type = typeof(List<OrderItemViewModel>))]
    //[ProducesResponseType(400)]
    //public async Task<IActionResult> GetItems(long id)
    //{
    //    try
    //    {
    //        var items = await _orderApplication.GetItemsAsync(id);
    //        return Ok(items);
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}
}




