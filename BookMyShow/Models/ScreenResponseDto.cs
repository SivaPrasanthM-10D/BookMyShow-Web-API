namespace BookMyShow.Models
{
    public class ScreenResponseDto
    {
        public Guid ScreenId { get; set; }
        public int ScreenNumber { get; set; }
        public Guid TheatreId { get; set; }
        public string TheatreName { get; set; }
        public Guid TheatreOwnerId { get; set; }
        public string TheatreOwnerName { get; set; }
        public List<ShowResponseDto> Shows { get; set; }
    }
}
