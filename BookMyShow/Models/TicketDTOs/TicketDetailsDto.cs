namespace BookMyShow.Models.TicketDTOs
{
    public class TicketDetailsDto
    {
        public required Guid TicketId { get; set; }
        public required string CustomerName { get; set; }
        public required string MovieTitle { get; set; }
        public required string ShowDate {  get; set; }
        public required string ShowTime { get; set; }
        public required string TheatreName { get; set; }
        public required int ScreenNumber { get; set; }
        public required List<int> SeatNo { get; set; }
        public required decimal TicketPrice { get; set; }
    }
}
