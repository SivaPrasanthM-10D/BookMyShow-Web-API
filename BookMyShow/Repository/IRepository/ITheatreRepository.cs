using BookMyShow.Data.Entities;
using BookMyShow.Models.TheatreDTOs;
using static BookMyShow.Models.TheatreDTOs.AddScreenResponseDto;

namespace BookMyShow.Repository.IRepository
{
    public interface ITheatreRepository
    {
        Task<List<User>> GetTheatreOwnersAsync();
        Task<TheatreOwnerSummaryDto?> GetTheatreAsync(Guid ownerid);
        Task<TheatreDto?> AddTheatreToTheatreOwnerAsync(Guid ownerid, AddTheatreDto addtheatredto);
        Task<string?> DeleteTheatreAsync(Guid ownerid);
    }
}