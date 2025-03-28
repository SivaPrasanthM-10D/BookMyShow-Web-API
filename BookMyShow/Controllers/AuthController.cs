using BookMyShow.Interfaces;
using BookMyShow.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            var result = userService.Register(registerDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var result = userService.Login(loginDto);
            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var result = userService.Logout();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
