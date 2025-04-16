using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Data.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        public string Password { get; internal set; }
        public string Email { get; internal set; }
        public string Role { get; internal set; }
        public string PhoneNumber { get; internal set; }
    }
}
