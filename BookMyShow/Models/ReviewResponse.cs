namespace BookMyShow.Models
{
    public class ReviewResponse
    {
        public required Guid ReviewId { get; set; }
        public required Guid UserId { get; set; }
        public required Guid MovieId { get; set; }
        public required double Rating { get; set; }
        public string? Review {  get; set; }
    }
}
