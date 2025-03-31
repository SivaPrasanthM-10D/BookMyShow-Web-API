using BookMyShow.Models;

namespace BookMyShow.Repository.IRepository
{
    public interface ITheatreManagementRepository
    {
        List<TheatreResponseDto> GetAllTheatres();
        List<ScreenResponseDto> GetAllScreens(Guid theatreid);
        List<ShowResponseDto> GetAllShows(Guid screenid);
        ScreenResponseDto? AddScreen(AddScreenDto addScreenDto);
        string? DeleteScreen(Guid screenId);
        ScreenResponseDto? UpdateScreen(Guid screenId, UpdateScreenDto updateScreenDto);
        ShowResponseDto? AddShow(AddShowDto addShowDto);
        string? DeleteShow(Guid showId);
        ShowResponseDto? UpdateShow(Guid showId, UpdateShowDto updateShowDto);
        string? RemoveTheatreofTheatreOwner(Guid ownerId);
    }
}
