using MasterManagment.Application.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostWebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductApplication _productApplication;

        public ProductController(IProductApplication productApplication)
        {
            _productApplication = productApplication;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductSearchCriteria searchModel)
        {
            var products = await _productApplication.Search(searchModel);
            return Ok(products ?? new List<ProductViewModel>());
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var product = await _productApplication.GetDetails(id);
            if (product == null)
                return NotFound(new { message = "محصول یافت نشد" });

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productApplication.CreateAsync(command);
            if (!result.IsSuccedded)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Edit(long id, [FromBody] EditProductCommand command)
        {
            command.Id = id;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result =await _productApplication.Edit(command);
            if (!result.IsSuccedded)
                return BadRequest(new { message = result.Message });

            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _productApplication.DeleteAsync(id);
            if (!result.IsSuccedded)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
