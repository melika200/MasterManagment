using System.Threading.Tasks;
using AccountManagment.Contracts.UserContracts;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHostAdminWebApi.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("admin/api/v{version:apiVersion}/[controller]")]
    //[Authorize(Roles = "Admin,Programmer")]  
    public class AccountController : ControllerBase
    {
        private readonly IUserApplication _userApplication;

        public AccountController(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        [HttpGet("AllUsers")]
        [ProducesResponseType(200, Type = typeof(List<UserViewModel>))]
        public async Task<ActionResult<List<UserViewModel>>> GetAllUsers()
        {


            var users = await _userApplication.GetAllUsers();

            //if (!RolesType.AllTypes.Any(r => r.Id == criteria.RoleId))
            //{
            //    return BadRequest("نقش انتخاب شده معتبر نیست.");
            //}

            return Ok(users);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(200, Type = typeof(EditUserViewModel))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserForEdit(long id)
        {
            var user =await _userApplication.GetForEdit(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [HttpPut("{id:long}")]
        public async Task<IActionResult> EditUser(long id, [FromBody] EditUserCommand command)
        {
            command.Id = id;
            var result = await _userApplication.Edit(command);
            if (!result.IsSuccedded)
                return BadRequest(new { message = result.Message });

            return NoContent();
        }

        //[HttpPut("{id:long}")]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400)]
        //public async Task<IActionResult> EditUser(long id, [FromBody] EditUserCommand command)
        //{
        //    command.Id = id;

        //    var result = await _userApplication.Edit(command);
        //    if (!result.IsSuccedded)
        //        return BadRequest(new { message = result.Message });

        //    return NoContent();
        //}

        [HttpDelete("{id:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var result = await _userApplication.DeleteAsync(id);
            if (!result.IsSuccedded)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPut("{id:long}/activate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ActivateUser(long id)
        {
            var result = await _userApplication.ActivateAsync(id);
            if (!result.IsSuccedded)
                return BadRequest(new { message = result.Message });

            return NoContent();
        }

        [HttpPut("{id:long}/deactivate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeactivateUser(long id)
        {
            var result = await _userApplication.DeactivateAsync(id);
            if (!result.IsSuccedded)
                return BadRequest(new { message = result.Message });

            return NoContent();
        }

        [HttpPut("{id:long}/changerole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangeUserRole(long id, [FromBody] ChangeUserRoleCommand command)
        {
            var result = await _userApplication.ChangeRoleAsync(id, command);
            if (!result.IsSuccedded)
                return BadRequest(new { message = result.Message });

            return NoContent();
        }

    }


}
