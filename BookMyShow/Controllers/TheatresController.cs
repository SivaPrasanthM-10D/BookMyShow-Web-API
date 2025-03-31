using BookMyShow.Data.Entities;
using BookMyShow.Models;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatresController : ControllerBase
    {
        private readonly ITheatreRepository _theatreOwnerRepository;

        public TheatresController(ITheatreRepository theatreOwnerRepository)
        {
            _theatreOwnerRepository = theatreOwnerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTheatreOwners()
        {
            List<User> theatreOwners = await _theatreOwnerRepository.GetTheatreOwnersAsync();
            return Ok(theatreOwners);
        }

        [HttpGet]
        [Route("{ownerid:guid}")]
        public async Task<IActionResult> GetTheatre(Guid ownerid)
        {
            TheatreOwnerSummaryDto? response = await _theatreOwnerRepository.GetTheatreAsync(ownerid);
            if (response == null)
            {
                return BadRequest("Theatre Owner not found.");
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("{ownerid:guid}")]
        public async Task<IActionResult> AddTheatreToTheatreOwner(Guid ownerid, AddTheatreDto addtheatredto)
        {
            AddScreenResponseDto.TheatreDto? response = await _theatreOwnerRepository.AddTheatreToTheatreOwnerAsync(ownerid, addtheatredto);
            if (response == null)
            {
                return BadRequest("Theatre Owner Not Found");
            }
            return Ok(response);
        }
    }
}