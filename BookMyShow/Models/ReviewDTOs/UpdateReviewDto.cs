using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Models.ReviewDTOs
{
    public class UpdateReviewDto
    {
        [Range(0, 10)]
        public required double Rating { get; set; }
        [StringLength(1000)]
        public string? Review { get; set; }
    }
}
