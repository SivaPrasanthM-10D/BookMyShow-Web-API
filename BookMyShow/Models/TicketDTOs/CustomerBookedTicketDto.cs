namespace BookMyShow.Models.TicketDTOs
{
    public class CustomerBookedTicketDto
    {
        public Guid TicketId { get; set; }
        public Guid ShowId { get; set; }
        public string MovieName { get; set; }
        public string ShowDate { get; set; }
        public string ShowTime { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
