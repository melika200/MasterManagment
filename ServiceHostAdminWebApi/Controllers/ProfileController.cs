using AccountManagment.Application.Contracts.Profile;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostAdminWebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("admin/api/v{version:apiVersion}/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileApplication _profileApplication;

        public ProfileController(IProfileApplication profileApplication)
        {
            _profileApplication = profileApplication;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profiles = await _profileApplication.GetAllProfileAsync(); 
            return Ok(profiles);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            var profile = await _profileApplication.GetProfileByIdAsync(id); 
            if (profile == null)
                return NotFound(new { message = "پروفایل یافت نشد." });
            return Ok(profile);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _profileApplication.DeleteProfileAsync(id); 
            if (!result.IsSuccedded)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
