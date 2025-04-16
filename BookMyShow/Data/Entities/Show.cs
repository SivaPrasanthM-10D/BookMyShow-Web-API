using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Data.Entities
{
    public class Show
    {
        [Key]
        public Guid ShowId { get; set; }

        [Required]
        public Guid ScreenId { get; set; }
        public Screen Screen { get; set; }

        [Required]
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }

        [Required]
        public List<int> AvailableSeats { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal TicketPrice { get; set; }

        [Required]
        public DateTime ShowDate { get; set; }

        [Required]
        public TimeSpan ShowTime { get; set; }
    }
}