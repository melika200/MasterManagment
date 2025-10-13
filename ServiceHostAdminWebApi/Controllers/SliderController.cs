using MasterManagement.Application.Contracts.SliderContracts;
using MasterManagment.Application.Contracts.Slider;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostAdminWebApi.Controllers;


[ApiController]
[ApiVersion("1.0")]
[Route("admin/api/v{version:apiVersion}/[controller]")]
//[Authorize(Roles = "Admin,Programmer")]
public class SliderController : ControllerBase
{
    private readonly ISliderApplication _sliderApplication;

    public SliderController(ISliderApplication sliderApplication)
    {
        _sliderApplication = sliderApplication;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<SliderViewModel>))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sliderApplication.GetSliderListAsync();
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetDetails(long id)
    {
        var slider = await _sliderApplication.GetDetailsAsync(id);
        if (slider == null)
            return NotFound();
        return Ok(slider);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromForm] CreateSliderCommand command)
    {
        var result = await _sliderApplication.CreateAsync(command);
        return Ok(result);
    }

    [HttpPut("edit/{id:long}")]
    public async Task<IActionResult> Edit(long id, [FromForm] EditSliderCommand command)
    {
        command.Id = id;
        var result = await _sliderApplication.EditAsync(command);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Remove(long id)
    {
        var result = await _sliderApplication.RemoveAsync(id);
        return Ok(result);
    }

    [HttpPatch("activate/{id:long}")]
    public async Task<IActionResult> Activate(long id)
    {
        var result = await _sliderApplication.ActivateAsync(id);
        return Ok(result);
    }

    [HttpPatch("deactivate/{id:long}")]
    public async Task<IActionResult> Deactivate(long id)
    {
        var result = await _sliderApplication.DeactivateAsync(id);
        return Ok(result);
    }
}
