using MasterManagment.Application.Contracts.CartItem;
using MasterManagment.Application.Contracts.Order;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartApplication _cartApplication;

    public CartController(ICartApplication cartApplication)
    {
        _cartApplication = cartApplication;
    }


    [HttpPost("create")]
    [ProducesResponseType(200, Type = typeof(long))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateCartCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var cartId = await _cartApplication.CreateAsync(command);
            return Ok(new { CartId = cartId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

  
    [HttpPut("edit")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Edit([FromBody] EditCartCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _cartApplication.EditAsync(command);
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
            await _cartApplication.CancelAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    ///   // دریافت مبلغ کارت بر اساس شناسه
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    [HttpGet("{id:long}/amount")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAmount(long id)
    {
        try
        {
            var amount = await _cartApplication.GetAmountByAsync(id);
            return Ok(amount);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

   
    [HttpGet("{id:long}/items")]
    [ProducesResponseType(200, Type = typeof(List<CartItemViewModel>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetItems(long id)
    {
        try
        {
            var items = await _cartApplication.GetItemsAsync(id);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

   
    [HttpPost("search")]
    [ProducesResponseType(200, Type = typeof(List<CartViewModel>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Search([FromBody] CartSearchCriteria criteria)
    {
        try
        {
            var carts = await _cartApplication.SearchAsync(criteria);
            return Ok(carts);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

  
    [HttpDelete("{id:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _cartApplication.DeleteAsync(id);
        if (!result.IsSuccedded)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
}
