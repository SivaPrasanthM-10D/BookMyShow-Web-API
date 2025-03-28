using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Data.Entities
{
    public class Theatre
    {
        [Key]
        public Guid TheatreId { get; set; }

        [Required]
        [StringLength(100)]
        public string TheatreName { get; set; }

        [Required]
        public Guid TheatreOwnerId { get; set; }
        public TheatreOwner TheatreOwner { get; set; }

        public ICollection<Screen> Screens { get; set; } = new List<Screen>();

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string Street { get; set; }
    }
}
