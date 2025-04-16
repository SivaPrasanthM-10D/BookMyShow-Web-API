namespace BookMyShow.Models.TheatreDTOs
{
    public class UpdateShowDto
    {
        public required Guid MovieId { get; set; }
        public required string ShowTime { get; set; }
        public required string ShowDate { get; set; }
        public required decimal TicketPrice { get; set; }
        public List<int> AvailableSeats { get; set; } = new List<int>();
    }
}
