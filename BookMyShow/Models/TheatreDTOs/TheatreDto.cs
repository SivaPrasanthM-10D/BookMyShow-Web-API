using BookMyShow.Models.TheatreDTOs;
public class TheatreDto
{
    public Guid TheatreId { get; set; }
    public string TheatreName { get; set; }
    public Guid TheatreOwnerId { get; set; }
    public string City { get; set; }
    public string Street { get; set; }

    public List<ScreenDto> Screens { get; set; } = new();
}
