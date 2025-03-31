using BookMyShow.Models;

namespace BookMyShow.Repository.IRepository
{
    public interface ITheatreManagementRepository
    {
        Task<List<TheatreResponseDto>> GetAllTheatresAsync();
        Task<List<ScreenResponseDto>> GetAllScreensAsync(Guid theatreid);
        Task<List<ShowResponseDto>> GetAllShowsAsync(Guid screenid);
        Task<ScreenResponseDto?> AddScreenAsync(AddScreenDto addScreenDto);
        Task<string?> DeleteScreenAsync(Guid screenId);
        Task<ScreenResponseDto?> UpdateScreenAsync(Guid screenId, UpdateScreenDto updateScreenDto);
        Task<ShowResponseDto?> AddShowAsync(AddShowDto addShowDto);
        Task<string?> DeleteShowAsync(Guid showId);
        Task<ShowResponseDto?> UpdateShowAsync(Guid showId, UpdateShowDto updateShowDto);
        Task<string?> RemoveTheatreofTheatreOwnerAsync(Guid ownerId);
    }
}
