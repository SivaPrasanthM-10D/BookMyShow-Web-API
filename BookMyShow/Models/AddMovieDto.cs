namespace BookMyShow.Models
{
    public class AddMovieDto
    {
        public required string Title { get; set; }
        public required string Genre { get; set; }
        public required int Duration { get; set; }
        public decimal Rating { get; set; }
        public required Guid AdminId { get; set; }
    }
}
