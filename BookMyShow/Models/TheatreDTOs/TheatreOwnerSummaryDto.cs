
namespace BookMyShow.Models.TheatreDTOs
{
    public class TheatreOwnerSummaryDto
    {
        public required Guid TheatreOwnerId { get; set; }
        public required string TheatreOwnerName { get; set; }
        public TheatreDto? Theatre {  get; set; }
    }
}
