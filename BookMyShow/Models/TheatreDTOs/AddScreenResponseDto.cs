namespace BookMyShow.Models.TheatreDTOs
{
    public class AddScreenResponseDto
    {
        public Guid ScreenId { get; set; }
        public int ScreenNumber { get; set; }
        public TheatreDto Theatre { get; set; }
        public List<ShowDto> Shows { get; set; }
    }
}
