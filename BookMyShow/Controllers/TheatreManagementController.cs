using BookMyShow.Exceptions;
using BookMyShow.Models.TheatreDTOs;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Retrieves all theatres.
        /// </summary>
        /// <returns>A list of theatres.</returns>
        [Authorize(Roles = "TheatreOwner,Customer")]
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

        /// <summary>
        /// Deletes a theatre by the theatre's ID.
        /// </summary>
        /// <param name="theatreid">The ID of the theatre.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        [Authorize(Roles = "TheatreOwner")]
        [HttpDelete]
        [Route("deleteTheatre/{theatreid:guid}")]
        public async Task<IActionResult> DeleteTheatreAsync(Guid theatreid)
        {
            try
            {
                string? result = await _theatreManagementRepository.DeleteTheatreAsync(theatreid);
                return Ok(result);
            }
            catch (TheatreOwnerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (TheatreNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all screens for a specific theatre.
        /// </summary>
        /// <param name="theatreid">The ID of the theatre to retrieve screens for.</param>
        /// <returns>A list of screens for the specified theatre.</returns>
        [Authorize(Roles = "TheatreOwner")]
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

        /// <summary>
        /// Retrieves all shows for a specific screen.
        /// </summary>
        /// <param name="screenid">The ID of the screen to retrieve shows for.</param>
        /// <returns>A list of shows for the specified screen.</returns>
        [Authorize(Roles = "TheatreOwner,Customer")]
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

        /// <summary>
        /// Adds a new screen to a theatre.
        /// </summary>
        /// <param name="addScreenDto">The screen details to add.</param>
        /// <returns>The added screen.</returns>
        /// <exception cref="NotFoundException">Thrown when the theatre is not found.</exception>
        [Authorize(Roles = "TheatreOwner")]
        [HttpPost]
        [Route("addScreen")]
        public async Task<IActionResult> AddScreen(AddScreenDto addScreenDto)
        {
            try
            {
                ScreenResponseDto? response = await _theatreManagementRepository.AddScreenAsync(addScreenDto);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a screen from the database.
        /// </summary>
        /// <param name="screenId">The ID of the screen to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <exception cref="NotFoundException">Thrown when the screen is not found.</exception>
        [Authorize(Roles = "TheatreOwner")]
        [HttpDelete]
        [Route("deleteScreen/{screenId:guid}")]
        public async Task<IActionResult> DeleteScreen(Guid screenId)
        {
            try
            {
                string? response = await _theatreManagementRepository.DeleteScreenAsync(screenId);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing screen in the database.
        /// </summary>
        /// <param name="screenId">The ID of the screen to update.</param>
        /// <param name="updateScreenDto">The updated screen details.</param>
        /// <returns>The updated screen.</returns>
        /// <exception cref="NotFoundException">Thrown when the screen is not found.</exception>
        [Authorize(Roles = "TheatreOwner")]
        [HttpPut]
        [Route("updateScreen/{screenId:guid}")]
        public async Task<IActionResult> UpdateScreen(Guid screenId, UpdateScreenDto updateScreenDto)
        {
            try
            {
                ScreenResponseDto? response = await _theatreManagementRepository.UpdateScreenAsync(screenId, updateScreenDto);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new show to a screen.
        /// </summary>
        /// <param name="addShowDto">The show details to add.</param>
        /// <returns>The added show.</returns>
        /// <exception cref="NotFoundException">Thrown when the screen is not found.</exception>
        [Authorize(Roles = "TheatreOwner")]
        [HttpPost]
        [Route("addShow")]
        public async Task<IActionResult> AddShow(AddShowDto addShowDto)
        {
            try
            {
                ShowResponseDto? response = await _theatreManagementRepository.AddShowAsync(addShowDto);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a show from the database.
        /// </summary>
        /// <param name="showId">The ID of the show to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <exception cref="NotFoundException">Thrown when the show is not found.</exception>
        [Authorize(Roles = "TheatreOwner")]
        [HttpDelete]
        [Route("deleteShow/{showId:guid}")]
        public async Task<IActionResult> DeleteShow(Guid showId)
        {
            try
            {
                string? response = await _theatreManagementRepository.DeleteShowAsync(showId);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing show in the database.
        /// </summary>
        /// <param name="showId">The ID of the show to update.</param>
        /// <param name="updateShowDto">The updated show details.</param>
        /// <returns>The updated show.</returns>
        /// <exception cref="NotFoundException">Thrown when the show is not found.</exception>
        [Authorize(Roles = "TheatreOwner")]
        [HttpPut]
        [Route("updateShow/{showId:guid}")]
        public async Task<IActionResult> UpdateShow(Guid showId, UpdateShowDto updateShowDto)
        {
            try
            {
                ShowResponseDto? response = await _theatreManagementRepository.UpdateShowAsync(showId, updateShowDto);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Removes a theatre from a theatre owner.
        /// </summary>
        /// <param name="ownerId">The ID of the theatre owner.</param>
        /// <returns>A message indicating the result of the removal.</returns>
        /// <exception cref="NotFoundException">Thrown when the theatre owner is not found.</exception>
        [Authorize(Roles = "TheatreOwner")]
        [HttpDelete]
        [Route("{ownerId:guid}")]
        public async Task<IActionResult> RemoveTheatreofTheatreOwner(Guid ownerId)
        {
            try
            {
                string? response = await _theatreManagementRepository.RemoveTheatreofTheatreOwnerAsync(ownerId);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}