using BookMyShow.Data.Entities;
using BookMyShow.Exceptions;
using BookMyShow.Models.TheatreDTOs;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers
{
    [Authorize(Roles = "TheatreOwner")]
    [Route("api/[controller]")]
    [ApiController]
    public class TheatresController : ControllerBase
    {
        private readonly ITheatreRepository _theatreOwnerRepository;

        public TheatresController(ITheatreRepository theatreOwnerRepository)
        {
            _theatreOwnerRepository = theatreOwnerRepository;
        }

        /// <summary>
        /// Retrieves all theatre owners.
        /// </summary>
        /// <returns>A list of theatre owners.</returns>
        [HttpGet]
        [Route("TheatreOwners")]
        public async Task<IActionResult> GetTheatreOwners()
        {
            try
            {
                List<User> theatreOwners = await _theatreOwnerRepository.GetTheatreOwnersAsync();
                if (!theatreOwners.Any())
                {
                    throw new NotFoundException("No theatre owners found.");
                }
                return Ok(theatreOwners);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a theatre owner by their ID.
        /// </summary>
        /// <param name="ownerid">The ID of the theatre owner to retrieve.</param>
        /// <returns>The theatre owner with the specified ID.</returns>
        [HttpGet]
        [Route("{ownerid:guid}")]
        public async Task<IActionResult> GetTheatre(Guid ownerid)
        {
            try
            {
                TheatreOwnerSummaryDto? response = await _theatreOwnerRepository.GetTheatreAsync(ownerid);
                if (response == null || response.Theatre == null)
                {
                    throw new NotFoundException("No theatres found.");
                }
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new theatre to a theatre owner.
        /// </summary>
        /// <param name="ownerid">The ID of the theatre owner.</param>
        /// <param name="addtheatredto">The theatre details to add.</param>
        /// <returns>The added theatre.</returns>
        [HttpPost]
        [Route("{ownerid}")]
        public async Task<IActionResult> AddTheatreToTheatreOwner(Guid ownerid, AddTheatreDto addtheatredto)
        {
            try
            {
                TheatreDto? response = await _theatreOwnerRepository.AddTheatreToTheatreOwnerAsync(ownerid, addtheatredto);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a theatre by the theatre owner's ID.
        /// </summary>
        /// <param name="ownerid">The ID of the theatre owner.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        [HttpDelete("{ownerid}")]
        public async Task<IActionResult> DeleteTheatre(Guid ownerid)
        {
            try
            {
                string? result = await _theatreOwnerRepository.DeleteTheatreAsync(ownerid);
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
    }
}