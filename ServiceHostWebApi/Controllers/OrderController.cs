using MasterManagment.Application.Contracts.Order;
using MasterManagment.Application.Contracts.OrderItem;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderApplication _orderApplication;

    public OrderController(IOrderApplication orderApplication)
    {
        _orderApplication = orderApplication;
    }


    [HttpPost("createFromCart")]
    [ProducesResponseType(200, Type = typeof(long))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateFromCart([FromBody] CreateOrderFromCartCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var orderId = await _orderApplication.FinalizeFromCartAsync(command.CartId, command.TransactionId!);
            return Ok(new { OrderId = orderId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

   
    [HttpGet("{id:long}/amount")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAmount(long id)
    {
        try
        {
            var amount = await _orderApplication.GetAmountByAsync(id);
            return Ok(amount);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// // دریافت اقلام سفارش بر اساس شناسه
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    [HttpGet("{id:long}/items")]
    [ProducesResponseType(200, Type = typeof(List<OrderItemViewModel>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetItems(long id)
    {
        try
        {
            var items = await _orderApplication.GetOrderItemsAsync(id);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    
    [HttpPost("search")]
    [ProducesResponseType(200, Type = typeof(List<OrderViewModel>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Search([FromBody] OrderSearchCriteria criteria)
    {
        try
        {
            var orders = await _orderApplication.SearchAsync(criteria);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

  
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
