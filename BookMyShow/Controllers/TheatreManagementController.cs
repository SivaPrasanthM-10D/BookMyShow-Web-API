using BookMyShow.Models;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatreManagementController : ControllerBase
    {
        public required ITheatreManagementRepository _theatreManagementRepository;

        public TheatreManagementController(ITheatreManagementRepository theatreManagementRepository)
        {
            _theatreManagementRepository = theatreManagementRepository;
        }

        [HttpGet]
        [Route("Theatres")]
        public async Task<IActionResult> GetAllTheatres()
        {
            List<TheatreResponseDto> theatres = await _theatreManagementRepository.GetAllTheatresAsync();
            if (!theatres.Any())
            {
                return NotFound("No theatres found.");
            }
            return Ok(theatres);
        }

        [HttpGet]
        [Route("Screens/{theatreid:guid}")]
        public async Task<IActionResult> GetAllScreens(Guid theatreid)
        {
            List<ScreenResponseDto> screens = await _theatreManagementRepository.GetAllScreensAsync(theatreid);
            if (!screens.Any())
            {
                return NotFound("No Screens found.");
            }
            return Ok(screens);
        }

        [HttpGet]
        [Route("Shows/{screenid:guid}")]
        public async Task<IActionResult> GetAllShows(Guid screenid)
        {
            List<ShowResponseDto> shows = await _theatreManagementRepository.GetAllShowsAsync(screenid);
            if (!shows.Any())
            {
                return NotFound("No shows found.");
            }
            return Ok(shows);
        }

        [HttpPost]
        [Route("addScreen")]
        public async Task<IActionResult> AddScreen(AddScreenDto addScreenDto)
        {
            ScreenResponseDto? response = await _theatreManagementRepository.AddScreenAsync(addScreenDto);
            if (response == null)
            {
                return BadRequest("Theatre not found. Can't add screen.");
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("deleteScreen/{screenId:guid}")]
        public async Task<IActionResult> DeleteScreen(Guid screenId)
        {
            string? response = await _theatreManagementRepository.DeleteScreenAsync(screenId);
            if (response == null)
            {
                return BadRequest("Screen not found.");
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("updateScreen/{screenId:guid}")]
        public async Task<IActionResult> UpdateScreen(Guid screenId, UpdateScreenDto updateScreenDto)
        {
            ScreenResponseDto? response = await _theatreManagementRepository.UpdateScreenAsync(screenId, updateScreenDto);
            if (response == null)
            {
                return BadRequest("Screen not found.");
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("addShow")]
        public async Task<IActionResult> AddShow(AddShowDto addShowDto)
        {
            ShowResponseDto? response = await _theatreManagementRepository.AddShowAsync(addShowDto);
            if (response == null)
            {
                return BadRequest("Screen not found");
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("deleteShow/{showId:guid}")]
        public async Task<IActionResult> DeleteShow(Guid showId)
        {
            string? response = await _theatreManagementRepository.DeleteShowAsync(showId);
            if (response == null)
            {
                return BadRequest("Show not found to delete.");
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("updateShow/{showId:guid}")]
        public async Task<IActionResult> UpdateShow(Guid showId, UpdateShowDto updateShowDto)
        {
            ShowResponseDto? response = await _theatreManagementRepository.UpdateShowAsync(showId, updateShowDto);
            if (response == null)
            {
                return BadRequest("Show not found. Can't update show.");
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("{ownerId:guid}")]
        public async Task<IActionResult> RemoveTheatreofTheatreOwner(Guid ownerId)
        {
            string? response = await _theatreManagementRepository.RemoveTheatreofTheatreOwnerAsync(ownerId);
            if (response == null)
            {
                return BadRequest("Theatre Owner not found.");
            }
            return Ok(response);
        }
    }
}