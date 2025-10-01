using MasterManagment.Application.Contracts.Order;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> CreateFromCart([FromBody] CreateOrderFromCartCommand command)
    {
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

    [HttpPut("edit")]
    public async Task<IActionResult> Edit([FromBody] EditOrderCommand command)
    {
        try
        {
            await _orderApplication.EditAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("cancel/{id}")]
    public async Task<IActionResult> Cancel(long id)
    {
        try
        {
            await _orderApplication.CancelAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}/amount")]
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

    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetItems(long id)
    {
        try
        {
            var items = await _orderApplication.GetItemsAsync(id);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("search")]
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
