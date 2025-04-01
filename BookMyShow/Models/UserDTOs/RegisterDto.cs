using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Models.UserDTOs
{
    public class RegisterDto
    {
        public required string UserName { get; set; }
        
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }

        [Phone]
        public required string PhoneNumber { get; set; }

        public required string Role { get; set; }
    }
}