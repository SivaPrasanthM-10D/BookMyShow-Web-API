using BookMyShow.Data.Entities;
using BookMyShow.Models;
using static BookMyShow.Models.AddScreenResponseDto;

namespace BookMyShow.Repository.IRepository
{
    public interface ITheatreRepository
    {
        List<User> GetTheatreOwners();
        TheatreOwnerSummaryDto? GetTheatre(Guid ownerid);
        TheatreDto? AddTheatreToTheatreOwner(Guid ownerid, AddTheatreDto addtheatredto);
    }
}
