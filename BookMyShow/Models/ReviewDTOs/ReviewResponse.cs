namespace BookMyShow.Models.ReviewDTOs
{
    public class ReviewResponse
    {
        public required Guid ReviewId { get; set; }
        public required Guid UserId { get; set; }
        public required string MovieTitle { get; set; }
        public required double Rating { get; set; }
        public string? Review {  get; set; }
    }
}
