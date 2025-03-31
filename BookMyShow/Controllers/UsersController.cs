using BookMyShow.Models;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("allUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("addUser")]
        public async Task<IActionResult> AddUser(AddUserDto addUserDto)
        {
            var user = await _userRepository.AddUsersAsync(addUserDto);
            if (user == null)
            {
                return BadRequest("Failed to add user");
            }
            return Ok(user);
        }

        [HttpDelete]
        [Route("deleteUser/{userid:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userid)
        {
            var result = await _userRepository.DeleteUserAsync(userid);
            if (result == null)
            {
                return NotFound("User not found");
            }
            return Ok(result);
        }
    }
}
