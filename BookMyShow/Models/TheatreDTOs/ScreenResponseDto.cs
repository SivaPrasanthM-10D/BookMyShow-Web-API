namespace BookMyShow.Models.TheatreDTOs
{
    public class ScreenResponseDto
    {
        public Guid ScreenId { get; set; }
        public int ScreenNumber { get; set; }
        public Guid TheatreId { get; set; }
        public string TheatreName { get; set; }
        public int TotalShows { get; set; }
    }
}
