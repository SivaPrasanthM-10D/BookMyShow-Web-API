namespace BookMyShow.Models.TheatreDTOs
{
    public class ShowDto
    {
        public Guid ShowId { get; set; }
        public Guid MovieId { get; set; }
        public TimeSpan ShowTime { get; set; }
        public DateTime ShowDate { get; set; }
        public List<int> AvailableSeats { get; set; }
        public decimal TicketPrice { get; set; }
    }
}
