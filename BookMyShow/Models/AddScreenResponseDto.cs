namespace BookMyShow.Models
{
    public class AddScreenResponseDto
    {
        public Guid ScreenId { get; set; }
        public int ScreenNumber { get; set; }
        public TheatreDto Theatre { get; set; }
        public List<ShowDto> Shows { get; set; }

        public class TheatreDto
        {
            public Guid TheatreId { get; set; }
            public string TheatreName { get; set; }
            public Guid TheatreOwnerId { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
        }

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
}
