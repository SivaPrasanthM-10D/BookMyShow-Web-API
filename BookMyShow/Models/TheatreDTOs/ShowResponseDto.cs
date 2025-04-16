namespace BookMyShow.Models.TheatreDTOs
{
    public class ShowResponseDto
    {
        public Guid ShowId { get; set; }
        public Guid ScreenId { get; set; }
        public int ScreenNumber { get; set; }
        public Guid TheatreId { get; set; }
        public string TheatreName { get; set; }
        public Guid MovieId { get; set; }
        public string ShowTime { get; set; }
        public string ShowDate { get; set; }
        public List<int> AvailableSeats { get; set; }
        public decimal TicketPrice { get; set; }
    }
}
