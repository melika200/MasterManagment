using AccountManagment.Application.Contracts.Profile;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion("1.0")]
[Route("admin/api/v{version:apiVersion}/[controller]")]
public class AdminProfileController : ControllerBase
{
    private readonly IProfileApplication _profileApplication;

    public AdminProfileController(IProfileApplication profileApplication)
    {
        _profileApplication = profileApplication;
    }

  
    [HttpGet("all")]
    [ProducesResponseType(200, Type = typeof(List<ProfileViewModel>))]
    public async Task<ActionResult<List<ProfileViewModel>>> GetAllProfiles()
    {
        var profiles = await _profileApplication.GetAllProfileAsync();
        return Ok(profiles.ToList());
    }

   
    [HttpGet("{userId:long}")]
    [ProducesResponseType(200, Type = typeof(ProfileViewModel))]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ProfileViewModel>> GetProfile(long userId)
    {
        var profile = await _profileApplication.GetProfileByUserIdInAdminAsync(userId);
        if (profile == null)
            return NotFound(new { message = "پروفایل یافت نشد." });
        return Ok(profile);
    }

   
    [HttpDelete("{id:long}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _profileApplication.DeleteProfileAsync(id);
        if (!result.IsSuccedded)
            return BadRequest(new { message = result.Message });

        return Ok(new { message = result.Message });
    }
}
