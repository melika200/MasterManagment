using MasterManagment.Application.Contracts.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostWebApi.Controllers;

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
    [ProducesResponseType(200, Type = typeof(List<ProductViewModel>))]
    public async Task<ActionResult<List<ProductViewModel>>> GetAll([FromQuery] ProductSearchCriteria searchModel)
    {
        var products = await _productApplication.Search(searchModel);
        return Ok(products ?? new List<ProductViewModel>());
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(200, Type = typeof(ProductViewModel))]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ProductViewModel>> GetById(long id)
    {
        var product = await _productApplication.GetDetails(id);
        if (product == null)
            return NotFound(new { message = "محصول یافت نشد" });

        return Ok(product);
    }

    [HttpGet("all-with-category")]
    [ProducesResponseType(200, Type = typeof(List<ProductViewModel>))]
    public async Task<ActionResult<List<ProductViewModel>>> GetAllProductsWithCategory()
    {
        var result = await _productApplication.GetAllProductsWithCategory();
        return Ok(result);
    }






}

    //[HttpPost]
    //[Authorize(Roles = "Admin,Programmer")]
    //[ProducesResponseType(200)]
    //[ProducesResponseType(400)]
    //public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    var result = await _productApplication.CreateAsync(command);
    //    if (!result.IsSuccedded)
    //        return BadRequest(new { message = result.Message });

    //    return Ok(new { message = result.Message });
    //}

    //[HttpPut("{id:long}")]
    //[Authorize(Roles = "Admin,Programmer")]
    //[ProducesResponseType(204)]
    //[ProducesResponseType(400)]
    //public async Task<IActionResult> Edit(long id, [FromBody] EditProductCommand command)
    //{
    //    command.Id = id;

    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    var result = await _productApplication.Edit(command);
    //    if (!result.IsSuccedded)
    //        return BadRequest(new { message = result.Message });

    //    return NoContent();
    //}

    //[HttpDelete("{id:long}")]
    //[Authorize(Roles = "Admin,Programmer")]
    //[ProducesResponseType(200)]
    //[ProducesResponseType(400)]
    //public async Task<IActionResult> Delete(long id)
    //{
    //    var result = await _productApplication.DeleteAsync(id);
    //    if (!result.IsSuccedded)
    //        return BadRequest(new { message = result.Message });

    //    return Ok(new { message = result.Message });
    //}

    //[HttpGet("all")]
    //[Authorize(Roles = "Admin,User,Programmer")]
    //[ProducesResponseType(200, Type = typeof(List<ProductViewModel>))]
    //public async Task<ActionResult<List<ProductViewModel>>> GetAllProducts()
    //{
    //    var result = await _productApplication.GetAllProducts();
    //    return Ok(result);
    //}