using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Data.Entities
{
    public class MovieReview
    {
        [Key]
        public Guid ReviewId { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; }

        [StringLength(1000)]
        public string? Review { get; set; }
    }
}
