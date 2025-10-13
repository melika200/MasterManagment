using MasterManagment.Application.Contracts.Gallery;
using MasterManagment.Application.Contracts.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostAdminWebApi.Controllers;


[ApiController]
[ApiVersion("1.0")]
[Route("admin/api/v{version:apiVersion}/[controller]")]
//[Authorize(Roles = "Admin,Programmer")] 
public class ProductController : ControllerBase
{
    private readonly IProductApplication _productApplication;
    private readonly IGalleryApplication _galleryApplication;

    public ProductController(IProductApplication productApplication, IGalleryApplication galleryApplication)
    {
        _productApplication = productApplication;
        _galleryApplication = galleryApplication;
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

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
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
    [ProducesResponseType(200, Type = typeof(ProductEditResponseCommand))] 
    [ProducesResponseType(400)]
    public async Task<IActionResult> Edit(long id, [FromBody] EditProductCommand command)
    {
        command.Id = id;

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _productApplication.Edit(command);
        if (!result.IsSuccedded)
            return BadRequest(new { message = result.Message });

       
        return Ok(result);
    }


    [HttpDelete("{id:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _productApplication.DeleteAsync(id);
        if (!result.IsSuccedded)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }

    [HttpGet("all-with-category")]
    [ProducesResponseType(200, Type = typeof(List<ProductViewModel>))]
    public async Task<ActionResult<List<ProductViewModel>>> GetAllProductsWithCategory()
    {
        var result = await _productApplication.GetAllProductsWithCategory();
        return Ok(result);
    }
    [HttpPost("upload-gallery-image")]
    public async Task<IActionResult> UploadGalleryImage([FromForm] UploadGalleryImageCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _galleryApplication.UploadImageAsync(command);

        if (!result.IsSuccedded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpDelete("gallery/{id:long}")]
    public async Task<IActionResult> DeleteGallery(long id)
    {
        var result = await _galleryApplication.DeleteAsync(id);
        if (!result.IsSuccedded)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpPut("gallery/{id:long}")]
    public async Task<IActionResult> EditGallery(long id, [FromBody] EditGalleryCommand command)
    {
        command.Id = id;
        if (id != command.Id)
            return BadRequest("شناسه گالری نامعتبر است.");

        var result = await _galleryApplication.EditAsync(command);
        if (!result.IsSuccedded)
            return BadRequest(result.Message);

        return NoContent();
    }


}




    //[HttpPut("{id:long}")]
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