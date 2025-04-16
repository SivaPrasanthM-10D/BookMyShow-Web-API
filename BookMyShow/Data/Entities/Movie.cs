using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Data.Entities
{
    public class Movie
    {
        [Key]
        public Guid MovieId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Genre { get; set; }

        [Required]
        [Range(1, 250)]
        public int Duration { get; set; }

        [Range(0, 5)]
        public decimal Rating { get; set; }

        [Required]
        public Guid AdminId { get; set; }
    }
}