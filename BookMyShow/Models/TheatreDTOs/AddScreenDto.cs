namespace BookMyShow.Models.TheatreDTOs
{
    public class AddScreenDto
    {
        public required Guid TheatreId { get; set; }
        public required int ScreenNumber { get; set; }
    }
}
