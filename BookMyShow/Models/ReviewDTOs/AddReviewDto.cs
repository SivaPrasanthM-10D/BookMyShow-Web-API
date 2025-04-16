using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Models.ReviewDTOs
{
    public class AddReviewDto
    {
        public required Guid UserId { get; set; }
        public required Guid MovieId { get; set; }
        [Range(0, 5)]
        public required double Rating { get; set; }
        [StringLength(1000)]
        public string? Review { get; set; }
    }
}
