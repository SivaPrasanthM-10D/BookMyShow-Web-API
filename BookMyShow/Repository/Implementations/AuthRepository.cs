using BookMyShow.Data.Entities;
using BookMyShow.Data;
using BookMyShow.Models;
using BookMyShow.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repository.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly BookMyShowDbContext dbContext;

        public AuthRepository(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ServiceResult> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                if (await dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
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

                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();

                return new ServiceResult { Success = true, Message = "User registered successfully" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<ServiceResult> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
                if (user == null || !VerifyPassword(loginDto.Password, user.Password))
                {
                    return new ServiceResult { Success = false, Message = "Invalid email or password" };
                }

                // Authentication successful
                return new ServiceResult { Success = true, Message = "Login successful" };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public Task<ServiceResult> LogoutAsync()
        {
            return Task.FromResult(new ServiceResult { Success = true, Message = "Logout successful" });
        }

        private bool VerifyPassword(string password, string storedPassword)
        {
            return password == storedPassword;
        }
    }
}