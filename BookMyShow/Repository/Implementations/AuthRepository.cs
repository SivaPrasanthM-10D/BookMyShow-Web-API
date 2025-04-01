using BookMyShow.Data;
using BookMyShow.Exceptions;
using BookMyShow.Models.CommonDTOs;
using BookMyShow.Models.UserDTOs;
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

        /// <summary>
        /// Authenticates a user based on the provided login credentials.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>A ServiceResult containing the authentication result and user information.</returns>
        /// <exception cref="UnauthorizedException">Thrown when the email or password is invalid.</exception>
        public async Task<ServiceResult> LoginAsync(LoginDto loginDto)
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            return new ServiceResult { Success = true, Message = "Login successful", User = user };
        }

        private bool VerifyPassword(string password, string storedPassword)
        {
            return password == storedPassword;
        }
    }
}