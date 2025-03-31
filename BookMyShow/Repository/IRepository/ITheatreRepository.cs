using BookMyShow.Data.Entities;
using BookMyShow.Models;
using static BookMyShow.Models.AddScreenResponseDto;

namespace BookMyShow.Repository.IRepository
{
    public interface ITheatreRepository
    {
        Task<List<User>> GetTheatreOwnersAsync();
        Task<TheatreOwnerSummaryDto?> GetTheatreAsync(Guid ownerid);
        Task<TheatreDto?> AddTheatreToTheatreOwnerAsync(Guid ownerid, AddTheatreDto addtheatredto);
    }
}