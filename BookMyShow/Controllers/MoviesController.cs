using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Interfaces;
using BookMyShow.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase, IMoviesController
    {
        private readonly BookMyShowDbContext dbContext;
        private readonly IReviewsController reviewsController;

        public MoviesController(BookMyShowDbContext dbContext, IReviewsController reviewsController)
        {
            this.dbContext = dbContext;
            this.reviewsController = reviewsController;
        }

        [HttpGet]
        public IActionResult GetAllMovies()
        {
            return Ok(dbContext.Movies.ToList());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetMovieById(Guid id)
        {
            Movie? movie = dbContext.Movies.Find(id);
            if (movie is null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpGet]
        [Route("{title}")]
        public IActionResult GetMovieByTitle(string title)
        {
            List<Movie> movies = dbContext.Movies.Where(movie => movie.Title == title).ToList();
            if (movies.Count == 0)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpPost]
        public IActionResult AddMovie(AddMovieDto addmoviedto)
        {
            var admin = dbContext.Admins.Find(addmoviedto.AdminId);
            if (admin == null)
            {
                return BadRequest("Invalid AdminId");
            }
            Movie movie = new Movie()
            {
                Title = addmoviedto.Title,
                Genre = addmoviedto.Genre,
                Duration = addmoviedto.Duration,
                Rating = addmoviedto.Rating,
                AdminId = addmoviedto.AdminId
            };

            dbContext.Movies.Add(movie);
            dbContext.SaveChanges();
            return Ok(movie);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateMovie(Guid id, UpdateMovieDto updatemoviedto)
        {
            Movie? movie = dbContext.Movies.Find(id);
            if (movie is null)
            {
                return NotFound();
            }
            movie.Title = updatemoviedto.Title;
            movie.Genre = updatemoviedto.Genre;
            movie.Duration = updatemoviedto.Duration;
            movie.Rating = updatemoviedto.Rating;
            dbContext.SaveChanges();
            return Ok(movie);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteMovie(Guid id)
        {
            Movie? movie = dbContext.Movies.Find(id);
            if (movie is null)
            {
                return NotFound();
            }
            dbContext.Movies.Remove(movie);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPatch]
        [Route("{id:guid}")]
        public IActionResult EditMovie(Guid id, [FromBody] JsonPatchDocument<Movie> patchmovie)
        {
            if (patchmovie is null)
            {
                return BadRequest();
            }
            Movie? movie = dbContext.Movies.Find(id);
            if (movie is null)
            {
                return NotFound();
            }
            patchmovie.ApplyTo(movie);
            dbContext.SaveChanges();
            return Ok(movie);
        }

        [HttpGet]
        [Route("reviews/{movieId:guid}")]
        public IActionResult GetReviewsByMovieId(Guid movieId)
        {
            return reviewsController.GetReviewsByMovieId(movieId);
        }

        [HttpDelete]
        [Route("deleteReview/{id:guid}")]
        public IActionResult DeleteReview(Guid id)
        {
            return reviewsController.DeleteReview(id);
        }
    }
}
