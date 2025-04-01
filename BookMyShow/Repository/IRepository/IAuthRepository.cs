using BookMyShow.Models.CommonDTOs;
using BookMyShow.Models.UserDTOs;

namespace BookMyShow.Repository.IRepository
{
    public interface IAuthRepository
    {
        //Task<ServiceResult> RegisterAsync(RegisterDto registerDto);
        Task<ServiceResult> LoginAsync(LoginDto loginDto);
        //Task<ServiceResult> LogoutAsync(Guid userid);
    }
}
