using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Interfaces;
using BookMyShow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatreOwnersController : ControllerBase, ITheatreOwnersController
    {
        private readonly BookMyShowDbContext dbContext;
        private readonly IMoviesController moviesController;
        private readonly IReviewsController reviewsController;

        public TheatreOwnersController(BookMyShowDbContext dbContext, IMoviesController MoviesController, IReviewsController reviewsController)
        {
            this.dbContext = dbContext;
            moviesController = MoviesController;
            this.reviewsController = reviewsController;
        }

        [HttpGet]
        [Route("Movies")]
        public IActionResult GetMovieList()
        {
            return moviesController.GetAllMovies();
        }

        [HttpGet]
        public IActionResult GetTheatreOwners()
        {
            IQueryable<User> TheatreOwners = dbContext.Users.Where(u => u.Role.Equals("TheatreOwner"));
            return Ok(TheatreOwners);
        }

        [HttpGet]
        [Route("{ownerid:guid}")]
        public IActionResult GetTheatre(Guid ownerid)
        {
            TheatreOwner? TheatreOwner = dbContext.TheatreOwners.Find(ownerid);
            if (TheatreOwner == null)
            {
                return BadRequest("Theatre Owner not found.");
            }
            return Ok(new
            {
                TheatreOwner.TheatreOwnerId,
                TheatreOwner.TheatreOwnerName,
                TheatreOwner.Theatre
            });
        }

        [HttpPost]
        [Route("{ownerid:guid}")]
        public IActionResult AddTheatreToTheatreOwner(Guid ownerid, AddTheatreDto addtheatredto)
        {
            TheatreOwner? theatreOwner = dbContext.TheatreOwners.Include(to => to.Theatre).FirstOrDefault(to => to.TheatreOwnerId == ownerid);
            if (theatreOwner == null)
            {
                return BadRequest("Theatre Owner Not Found");
            }
            Theatre theatre = new()
            {
                TheatreName = addtheatredto.TheatreName,
                Street = addtheatredto.Street,
                City = addtheatredto.City,
                TheatreOwnerId = theatreOwner.TheatreOwnerId
            };
            dbContext.Theatres.Add(theatre);
            theatreOwner.Theatre = theatre;
            dbContext.SaveChanges();
            return Ok(new
            {
                theatre.TheatreId,
                theatre.TheatreName,
                theatre.Street,
                theatre.City,
                theatre.TheatreOwnerId
            });
        }

        [HttpGet]
        [Route("reviews/{movieId:guid}")]
        public IActionResult GetReviewsByMovieId(Guid movieId)
        {
            IActionResult result = reviewsController.GetReviewsByMovieId(movieId);
            return result ?? NotFound();
        }
    }
}