using MasterManagment.Application.Contracts.ProductCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ServiceHostAdminWebApi.Controllers;



    [ApiController]
    [ApiVersion("1.0")]
    [Route("admin/api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Admin,Programmer")] 
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryApplication _productCategoryApplication;

        public ProductCategoryController(IProductCategoryApplication productCategoryApplication)
        {
            _productCategoryApplication = productCategoryApplication;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProductCategoryViewModel>))]
        public ActionResult<List<ProductCategoryViewModel>> Get([FromQuery] ProductCategorySearchModel searchModel)
        {
            var result = _productCategoryApplication.Search(searchModel);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(EditProductCategoryCommand))]
        [ProducesResponseType(404)]
        public ActionResult<EditProductCategoryCommand> Get(long id)
        {
            var details = _productCategoryApplication.GetDetails(id);
            if (details == null)
                return NotFound();

            return Ok(details);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateProductCategoryCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productCategoryApplication.CreateAsync(command);
            if (!result.IsSuccedded)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Edit(long id, [FromBody] EditProductCategoryCommand command)
        {
            command.Id = id;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productCategoryApplication.EditAsync(command);
            if (!result.IsSuccedded)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _productCategoryApplication.DeleteAsync(id);
            if (!result.IsSuccedded)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(List<ProductCategoryViewModel>))]
        public async Task<ActionResult<List<ProductCategoryViewModel>>> GetAll()
        {
            var result = await _productCategoryApplication.GetAll();
            return Ok(result);
        }
    }

