using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Data.Entities
{
    public class Screen
    {
        [Key]
        public Guid ScreenId { get; set; }

        [Required]
        public int ScreenNumber { get; set; }

        [Required]
        public Guid TheatreId { get; set; }
        public Theatre Theatre { get; set; }

        public ICollection<Show> Shows { get; set; } = new List<Show>();
    }
}
