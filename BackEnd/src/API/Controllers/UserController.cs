using API.Common;
using API.Services.Requests;
using API.Services.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserRequest userInput)
        {
            var result = await userService.LoginAsync(userInput);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(GlobalConstants.LOG_IN_ERROR);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await userService.LogoutAsync();
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest model)
        {
            var result = await userService.RegisterAsync(model);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(GlobalConstants.REGISTER_ERROR);
        }

        [HttpPost("register/librarian")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> RegisterLibrarian([FromBody] RegisterUserRequest request)
        {
            var result = await userService.RegisterLibrarianAsync(request);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromForm] string email)
        {
            var result = await userService.ResetPasswordAsync(email);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest(GlobalConstants.SEND_EMAIL_FAIL);
        }
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ChangePassword([FromForm] string email, [FromForm] string password)
        {
            var result = await userService.ChangePasswordAsync(email, password);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest(GlobalConstants.CHANGE_PASSWORD_FAIL);
        }
        //[HttpGet("all")]
        //public async Task<IActionResult> GetAllUsersInPage(int UsersPerPage,int Page)
        //{

        //}
    }
}
