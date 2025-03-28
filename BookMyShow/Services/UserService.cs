using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Interfaces;
using BookMyShow.Models;

namespace BookMyShow.Services
{
    public class UserService : IUserService
    {
        private readonly BookMyShowDbContext dbContext;

        public UserService(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ServiceResult Register(RegisterDto registerDto)
        {
            if (dbContext.Users.Any(u => u.Email == registerDto.Email))
            {
                return new ServiceResult { Success = false, Message = "Email already exists" };
            }

            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = registerDto.Password,
                Role = "Customer" // Default role
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return new ServiceResult { Success = true, Message = "User registered successfully" };
        }

        public ServiceResult Login(LoginDto loginDto)
        {
            var user = dbContext.Users.SingleOrDefault(u => u.Email == loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
            {
                return new ServiceResult { Success = false, Message = "Invalid email or password" };
            }

            // Authentication successful
            return new ServiceResult { Success = true, Message = "Login successful" };
        }

        public ServiceResult Logout()
        {
            return new ServiceResult { Success = true, Message = "Logout successful" };
        }

        private bool VerifyPassword(string password, string storedPassword)
        {
            return password == storedPassword;
        }
    }
}
