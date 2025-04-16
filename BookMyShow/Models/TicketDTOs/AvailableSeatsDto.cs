namespace BookMyShow.Models.TicketDTOs
{
    public class AvailableSeatsDto
    {
        public required int TotalSeatsAvailable { get; set; }
        public required List<int> AvailableSeats { get; set; }
    }
}