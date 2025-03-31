namespace BookMyShow.Models
{
    public class BookedTicketsDto
    {
        public required Guid TicketId { get; set; }
        public required Guid ShowId { get; set; }
        public required string MovieName { get; set; }
        public required string ShowDate { get; set; }
        public required string ShowTime { get; set; }
        public required Guid CustomerId { get; set; }
        public required string CustomerName { get; set; }
    }
}
