using BookMyShow.Data.Entities;
using BookMyShow.Exceptions;
using BookMyShow.Models.UserDTOs;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A list of users.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                List<User> users = await _userRepository.GetAllUsersAsync();
                if (!users.Any())
                {
                    throw new NotFoundException("No users found.");
                }
                return Ok(users);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="addUserDto">The user details to add.</param>
        /// <returns>The added user.</returns>
        /// <exception cref="BadRequestException">Thrown when the user data is invalid.</exception>
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto addUserDto)
        {
            try
            {
                User? user = await _userRepository.AddUsersAsync(addUserDto);
                return Ok(user);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="userid">The ID of the user to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <exception cref="NotFoundException">Thrown when the user is not found.</exception>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{userid:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userid)
        {
            try
            {
                string? result = await _userRepository.DeleteUserAsync(userid);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
