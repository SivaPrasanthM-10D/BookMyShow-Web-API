namespace BookMyShow.Models.TheatreDTOs
{
    public class TheatreResponseDto
    {
        public required Guid TheatreId { get; set; }
        public required string TheatreName { get; set; }
        public required Guid TheatreOwnerId { get; set; }
        public required string TheatreOwnerName { get; set; }
        public required int ScreenCount { get; set; }
        public required string Street {  get; set; }
        public required string City { get; set; }
    }
}
