using BookMyShow.Data.Entities;
using static BookMyShow.Models.AddScreenResponseDto;

namespace BookMyShow.Models
{
    public class TheatreOwnerSummaryDto
    {
        public required Guid TheatreOwnerId { get; set; }
        public required string TheatreOwnerName { get; set; }
        public TheatreDto? Theatre {  get; set; }
    }
}
