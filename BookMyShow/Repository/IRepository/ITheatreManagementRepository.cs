using BookMyShow.Models.TheatreDTOs;

namespace BookMyShow.Repository.IRepository
{
    public interface ITheatreManagementRepository
    {
        Task<List<TheatreResponseDto>> GetAllTheatresAsync();
        Task<string?> DeleteTheatreAsync(Guid theatreid);
        Task<List<ScreenResponseDto>> GetAllScreensAsync(Guid theatreid);
        Task<List<ShowResponseDto>> GetAllShowsAsync(Guid screenid);
        Task<ShowResponseDto> GetShowAsync(Guid showid);
        Task<List<ShowResponseDto>> GetAllShowsByMovieNameAsync(string movieName);
        Task<ScreenResponseDto?> AddScreenAsync(AddScreenDto addScreenDto);
        Task<string?> DeleteScreenAsync(Guid screenId);
        Task<ScreenResponseDto?> UpdateScreenAsync(Guid screenId, UpdateScreenDto updateScreenDto);
        Task<ShowResponseDto?> AddShowAsync(AddShowDto addShowDto);
        Task<string?> DeleteShowAsync(Guid showId);
        Task<ShowResponseDto?> UpdateShowAsync(Guid showId, UpdateShowDto updateShowDto);
        Task<string?> RemoveTheatreofTheatreOwnerAsync(Guid ownerId);
    }
}
