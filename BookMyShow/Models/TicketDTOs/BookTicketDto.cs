namespace BookMyShow.Models
{
    public class BookTicketDto
    {
        public required Guid CustomerId { get; set; }
        public required Guid ShowId { get; set; }
        public List<int> SeatNo { get; set; } = new List<int>();
    }
}
