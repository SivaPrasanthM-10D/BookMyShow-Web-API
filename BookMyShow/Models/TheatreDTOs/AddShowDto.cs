namespace BookMyShow.Models.TheatreDTOs
{
    public class AddShowDto
    {
        public required Guid ScreenId { get; set; }
        public required Guid MovieId { get; set; }
        public required string ShowTime { get; set; }
        public required string ShowDate { get; set; }
        public required int TotalSeats { get; set; }
        public required decimal TicketPrice { get; set; }
    }
}
