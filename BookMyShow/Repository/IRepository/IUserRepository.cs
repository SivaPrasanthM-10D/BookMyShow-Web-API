using BookMyShow.Data.Entities;
using BookMyShow.Models.UserDTOs;

namespace BookMyShow.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> AddUsersAsync(AddUserDto adduserdto);
        Task<string?> DeleteUserAsync(Guid userid);
        Task<User?> GetUserAsync(Guid userid);
    }
}