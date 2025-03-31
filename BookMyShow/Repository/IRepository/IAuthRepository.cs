using BookMyShow.Models;

namespace BookMyShow.Repository.IRepository
{
    public interface IAuthRepository
    {
        Task<ServiceResult> RegisterAsync(RegisterDto registerDto);
        Task<ServiceResult> LoginAsync(LoginDto loginDto);
        Task<ServiceResult> LogoutAsync();
    }
}
