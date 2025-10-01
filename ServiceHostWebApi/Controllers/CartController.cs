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
        public async Task<IActionResult> Create([FromBody] CreateCartCommand command)
        {
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
        public async Task<IActionResult> Edit([FromBody] EditCartCommand command)
        {
            try
            {
                await _cartApplication.EditAsync(command);
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
                await _cartApplication.CancelAsync(id);
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
                var amount = await _cartApplication.GetAmountByAsync(id);
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
                var items = await _cartApplication.GetItemsAsync(id);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("search")]
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
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _cartApplication.DeleteAsync(id);
            if (!result.IsSuccedded)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }





