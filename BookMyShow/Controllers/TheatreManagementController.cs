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
        public IActionResult GetAllTheatres()
        {
            List<TheatreResponseDto> theatres = _theatreManagementRepository.GetAllTheatres();
            if (!theatres.Any())
            {
                return NotFound("No theatres found.");
            }
            return Ok(theatres);
        }

        [HttpGet]
        [Route("Screens/{theatreid:guid}")]
        public IActionResult GetAllScreens(Guid theatreid)
        {
            List<ScreenResponseDto> screens = _theatreManagementRepository.GetAllScreens(theatreid);
            if(!screens.Any())
            {
                return NotFound("No Screens found.");
            }
            return Ok(screens);
        }

        [HttpGet]
        [Route("Shows/{screenid:guid}")]
        public IActionResult GetAllShows(Guid screenid)
        {
            List<ShowResponseDto> shows = _theatreManagementRepository.GetAllShows(screenid);

            if (!shows.Any()) return NotFound("No shows found.");
            return Ok(shows);
        }

        [HttpPost]
        [Route("addScreen")]
        public IActionResult AddScreen(AddScreenDto addScreenDto)
        {
            ScreenResponseDto? response = _theatreManagementRepository.AddScreen(addScreenDto);
            if(response is null) return BadRequest("Theatre not found. Can't add screen.");
            return Ok(response);
        }

        [HttpDelete]
        [Route("deleteScreen/{screenId:guid}")]
        public IActionResult DeleteScreen(Guid screenId)
        {
            string? response = _theatreManagementRepository.DeleteScreen(screenId);
            if (response == null)
            {
                return BadRequest("Screen not found.");
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("updateScreen/{screenId:guid}")]
        public IActionResult UpdateScreen(Guid screenId, UpdateScreenDto updateScreenDto)
        {
            ScreenResponseDto? response = _theatreManagementRepository.UpdateScreen(screenId, updateScreenDto);
            if (response == null)
            {
                return BadRequest("Screen not found.");
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("addShow")]
        public IActionResult AddShow(AddShowDto addShowDto)
        {
            ShowResponseDto? response = _theatreManagementRepository.AddShow(addShowDto);

            if (response == null)
            {
                return BadRequest("Screen not found");
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("deleteShow/{showId:guid}")]
        public IActionResult DeleteShow(Guid showId)
        {
            string? response = _theatreManagementRepository.DeleteShow(showId);
            if (response == null)
            {
                return BadRequest("Show not found to delete.");
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("updateShow/{showId:guid}")]
        public IActionResult UpdateShow(Guid showId, UpdateShowDto updateShowDto)
        {
            ShowResponseDto? response = _theatreManagementRepository.UpdateShow(showId, updateShowDto);
            if (response == null)
            {
                return BadRequest("Show not found. Can't update show.");
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("{ownerId:guid}")]
        public IActionResult RemoveTheatreofTheatreOwner(Guid ownerId)
        {
            string? response = _theatreManagementRepository.RemoveTheatreofTheatreOwner(ownerId);
            if (response is null)
            {
                return BadRequest("Theatre Owner not found.");
            }
            return Ok(response);
        }
    }
}