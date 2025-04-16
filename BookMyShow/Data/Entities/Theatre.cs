using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey(nameof(TheatreOwnerId))]
        public TheatreOwner TheatreOwner { get; set; } = null!;

        public ICollection<Screen> Screens { get; set; } = new List<Screen>();

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string Street { get; set; }
    }
}
