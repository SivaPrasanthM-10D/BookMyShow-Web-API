using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookMyShow.Data.Entities;
using BookMyShow.Exceptions;
using BookMyShow.Models.CommonDTOs;
using BookMyShow.Models.UserDTOs;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookMyShow.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(RegisterDto registerDto)
        //{
        //    try
        //    {
        //        ServiceResult result = await _authRepository.RegisterAsync(registerDto);
        //        return Ok(result);
        //    }
        //    catch (BadRequestException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>An IActionResult containing the JWT token and user information.</returns>
        /// <exception cref="UnauthorizedException">Thrown when the login credentials are invalid.</exception>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                ServiceResult<User> result = await _authRepository.LoginAsync(loginDto);
                if (result.Success)
                {
                    User user = result.Data!;
                    var userRole = user.Role;
                    var authClaims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Role, userRole)
                    };
                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        expires: DateTime.Now.AddMinutes(60),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                        SecurityAlgorithms.HmacSha256
                        )
                        );

                    return Ok(new ServiceResult<string> {Success = true, StatusCode = 200, Message = "Ok", Data = new JwtSecurityTokenHandler().WriteToken(token)});
                }
                throw new UnauthorizedException("Invalid email or password");
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new ServiceResult<string> { Success = false, StatusCode = 401, Message = ex.Message, Data = null });
            }
        }

        //[HttpPost("logout/{userid:guid}")]
        //public async Task<IActionResult> Logout(Guid userid)
        //{
        //    ServiceResult result = await _authRepository.LogoutAsync(userid);
        //    return Ok(result);
        //}
    }
}
