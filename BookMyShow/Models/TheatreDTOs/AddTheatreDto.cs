namespace BookMyShow.Models.TheatreDTOs
{
    public class AddTheatreDto
    {
        public required string TheatreName { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
    }
}
