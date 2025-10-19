using System.Security.Claims;
using MasterManagment.Application.Contracts.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class SupportController : ControllerBase
{
    private readonly ISupportApplication _supportApplication;

    public SupportController(ISupportApplication supportApplication)
    {
        _supportApplication = supportApplication;
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateSupportCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var accountIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(accountIdClaim)) return Unauthorized();

        var accountId = long.Parse(accountIdClaim);
        var result = await _supportApplication.CreateAsync(command, accountId);

        if (!result.IsSuccedded) return BadRequest(result.Message);
        return Ok(result.Message);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Edit(long id, [FromBody] EditUserSupportCommand model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        model.Id = id;
        var result = await _supportApplication.EditUserTicketAsync(model, User);
        if (!result.IsSuccedded) return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpGet("my-tickets")]
    [ProducesResponseType(200, Type = typeof(List<SupportViewModel>))]
    public async Task<IActionResult> GetMyTickets()
    {
        var accountIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(accountIdClaim)) return Unauthorized();

        var accountId = long.Parse(accountIdClaim);
        var tickets = await _supportApplication.GetUserTickets(accountId);

        return Ok(tickets);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(200, Type = typeof(SupportViewModel))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(long id)
    {
        var ticket = await _supportApplication.GetDetails(id);
        if (ticket == null) return NotFound();
        return Ok(ticket);
    }
}
