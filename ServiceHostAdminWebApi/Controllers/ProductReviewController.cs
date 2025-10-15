using MasterManagment.Application.Contracts.ProductReview;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostAdminWebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("admin/api/v{version:apiVersion}/[controller]")]

    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewApplication _productReviewApplication;

        public ProductReviewController(IProductReviewApplication productReviewApplication)
        {
            _productReviewApplication = productReviewApplication;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _productReviewApplication.GetAllProductReviewAsync();
            return Ok(reviews);
        }

    
        [HttpPut("{id}/confirm")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Confirm(long id)
        {
            var result = await _productReviewApplication.ConfirmProductReviewAsync(id);
            if (!result.IsSuccedded)
                return NotFound(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPut("{id}/unconfirm")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UnConfirm(long id)
        {
            var result = await _productReviewApplication.UnConfirmProductReviewAsync(id);
            if (!result.IsSuccedded)
                return NotFound(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

      
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Remove(long id)
        {
            var result = await _productReviewApplication.RemoveProductReviewAsync(id);
            if (!result.IsSuccedded)
                return NotFound(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

      
        [HttpGet("byproduct/{productId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByProduct(long productId)
        {
            var reviews = await _productReviewApplication.GetProductReviewByProductIdAsync(productId);
            if (reviews == null || !reviews.Any())
                return NotFound(new { message = "هیچ نظری برای این محصول یافت نشد." });

            return Ok(reviews);
        }
    }
}
