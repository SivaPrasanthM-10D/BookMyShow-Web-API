namespace BookMyShow.Models.TheatreDTOs
{
    public class ScreenDto
    {
        public Guid ScreenId { get; set; }
        public int ScreenNumber { get; set; }
        public Guid TheatreId { get; set; }
        public List<ShowDto> Shows { get; set; } = new();
    }
}
