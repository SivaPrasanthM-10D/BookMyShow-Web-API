using BookMyShow.Models;

namespace BookMyShow.Interfaces
{
    public interface IUserService
    {
        ServiceResult Register(RegisterDto registerDto);
        ServiceResult Login(LoginDto loginDto);
        ServiceResult Logout();
    }
}