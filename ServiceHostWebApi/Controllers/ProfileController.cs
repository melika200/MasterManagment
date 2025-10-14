using AccountManagment.Application.Contracts.Profile;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostWebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileApplication _profileApplication;

    public ProfileController(IProfileApplication profileApplication)
    {
        _profileApplication = profileApplication;
    }

    [HttpGet("{userId:long}")]
    public async Task<IActionResult> GetProfile(long userId)
    {
        var profile = await _profileApplication.GetProfileByUserIdAsync(userId);
        if (profile == null)
            return NotFound(new { message = "پروفایل یافت نشد." });
        return Ok(profile);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProfileCommand command)
    {
        var result = await _profileApplication.CreateProfileAsync(command);
        if (!result.IsSuccedded)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] EditProfileCommand command)
    {
        var result = await _profileApplication.EditProfileAsync(command);
        if (!result.IsSuccedded)
            return BadRequest(result);
        return Ok(result);
    }
}
