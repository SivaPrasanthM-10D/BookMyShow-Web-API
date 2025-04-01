namespace BookMyShow.Models.TheatreDTOs
{
    public class ShowSummaryDto
    {
        public Guid ShowId { get; set; }
        public Guid MovieId { get; set; }
        public string ShowTime { get; set; }
        public string ShowDate { get; set; }
        public int SeatsAvailableCount { get; set; }
        public decimal TicketPrice { get; set; }
    }
}
