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
        public IActionResult GetTheatreOwners()
        {
            List<User> TheatreOwners = _theatreOwnerRepository.GetTheatreOwners();
            return Ok(TheatreOwners);
        }

        [HttpGet]
        [Route("{ownerid:guid}")]
        public IActionResult GetTheatre(Guid ownerid)
        {
            TheatreOwnerSummaryDto? response = _theatreOwnerRepository.GetTheatre(ownerid);
            if (response == null)
            {
                return BadRequest("Theatre Owner not found.");
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("{ownerid:guid}")]
        public IActionResult AddTheatreToTheatreOwner(Guid ownerid, AddTheatreDto addtheatredto)
        {
            AddScreenResponseDto.TheatreDto? response = _theatreOwnerRepository.AddTheatreToTheatreOwner(ownerid, addtheatredto);
            if (response == null)
            {
                return BadRequest("Theatre Owner Not Found");
            }
            return Ok(response);
        }
    }
}