using AccountManagment.Application.Contracts.Profile;
using Microsoft.AspNetCore.Mvc;

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


    [HttpGet("myprofile")]
    [ProducesResponseType(200, Type = typeof(ProfileViewModel))]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ProfileViewModel>> GetMyProfile()
    {
        var profile = await _profileApplication.GetProfileByUserIdAsync(User);
        if (profile == null)
            return NotFound(new { message = "پروفایل یافت نشد." });
        return Ok(profile);
    }

 
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateProfileCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _profileApplication.CreateProfileAsync(command, User);
        if (!result.IsSuccedded)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }

  
    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Edit([FromBody] EditProfileCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _profileApplication.EditProfileAsync(command, User);
        if (!result.IsSuccedded)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
}
