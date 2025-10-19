using MasterManagment.Application.Contracts.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostAdminWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("admin/api/v{version:apiVersion}/[controller]")]
//[Authorize(Roles = "Admin,Programmer")]
public class SupportController : ControllerBase
{
    private readonly ISupportApplication _supportApplication;

    public SupportController(ISupportApplication supportApplication)
    {
        _supportApplication = supportApplication;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<SupportViewModel>))]
    public async Task<ActionResult<List<SupportViewModel>>> GetAll()
    {
        var result = await _supportApplication.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(200, Type = typeof(SupportViewModel))]
    [ProducesResponseType(404)]
    public async Task<ActionResult<SupportViewModel>> Get(long id)
    {
        var details = await _supportApplication.GetDetails(id);
        if (details == null) return NotFound();

        return Ok(details);
    }

    [HttpPut("{id:long}/status")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> ChangeStatus(long id, [FromBody] EditAdminSupportCommand command)
    {
        command.Id = id;
        var result = await _supportApplication.EditAdminTicketAsync(command);
        if (!result.IsSuccedded) return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _supportApplication.DeleteAsync(id);
        if (!result.IsSuccedded) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
