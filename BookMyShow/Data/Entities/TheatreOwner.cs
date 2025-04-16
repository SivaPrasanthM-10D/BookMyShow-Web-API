using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Data.Entities
{
    public class TheatreOwner
    {
        [Key]
        public Guid TheatreOwnerId { get; set; }

        [StringLength(100)]
        public required string TheatreOwnerName { get; set; }

        public ICollection<Theatre> Theatres { get; set; } = new List<Theatre>();
    }
}
