using MasterManagement.Application.Contracts.SliderContracts;
using MasterManagment.Application.Contracts.Slider;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SliderController : ControllerBase
{
    private readonly ISliderApplication _sliderApplication;

    public SliderController(ISliderApplication sliderApplication)
    {
        _sliderApplication = sliderApplication;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<SliderViewModel>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sliderApplication.GetSliderListAsync();
        return Ok(result);
    }

}
