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
                ServiceResult result = await _authRepository.LoginAsync(loginDto);
                if (result.Success)
                {
                    User user = result.User;
                    var userRole = user.Role;
                    var authClaims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Role, userRole)
                    };
                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        expires: DateTime.Now.AddMinutes(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                        SecurityAlgorithms.HmacSha256
                        )
                        );

                    return Ok(new {Token = new JwtSecurityTokenHandler().WriteToken(token), user});
                }
                throw new UnauthorizedException("Invalid Credentials. Login failed.");
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
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
