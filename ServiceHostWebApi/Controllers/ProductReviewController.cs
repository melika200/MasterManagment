using MasterManagment.Application.Contracts.ProductReview;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductReviewController : ControllerBase
{
    private readonly IProductReviewApplication _application;

    public ProductReviewController(IProductReviewApplication application)
    {
        _application = application;
    }

    [HttpGet("{productId:long}")]
    public async Task<IActionResult> GetByProduct(long productId)
    {
        var result = await _application.GetProductReviewByProductIdAsync(productId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductReviewCommand command)
    {
        var result = await _application.CreateProductReviewAsync(command);
        return Ok(result);
    }
}
