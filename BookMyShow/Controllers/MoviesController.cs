using BookMyShow.Data.Entities;
using BookMyShow.Models;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet]
        public IActionResult GetAllMovies()
        {
            Task<List<Movie>?> movies = _movieRepository.GetAllMoviesAsync();
            if (movies is null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetMovieById(Guid id)
        {
            Task<Movie?> movie = _movieRepository.GetMovieByIdAsync(id);
            if(movie is null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpGet]
        [Route("{title}")]
        public IActionResult GetMovieByTitle(string title)
        {
            Task<List<Movie>?> movies = _movieRepository.GetMovieByTitleAsync(title);
            if (movies is null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpPost]
        public IActionResult AddMovie(AddMovieDto addmoviedto)
        {
            Task<Movie?> result = _movieRepository.AddMovieAsync(addmoviedto);
            if (result == null)
            {
                return BadRequest("Invalid AdminId");
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateMovie(Guid id, UpdateMovieDto updatemoviedto)
        {
            Task<Movie?> movie = _movieRepository.UpdateMovieAsync(id, updatemoviedto);
            if (movie is null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteMovie(Guid id)
        {
            Task<string?> result = _movieRepository.DeleteMovieAsync(id);
            if (result is null)
            {
                return NotFound("Movie not found.");
            }
            return Ok(result);
        }

        [HttpPatch]
        [Route("{id:guid}")]
        public IActionResult EditMovie(Guid id, [FromBody] JsonPatchDocument<Movie> patchmovie)
        {
            if (patchmovie is null)
            {
                return BadRequest();
            }
            Task<Movie?> movie = _movieRepository.EditMovieAsync(id, patchmovie);
            if (movie is null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpGet]
        [Route("reviews/{movieId:guid}")]
        public IActionResult GetReviewsByMovieId(Guid movieId)
        {
           Task<List<ReviewResponse>?> review = _movieRepository.GetReviewsByMovieIdAsync(movieId);
           if(review is null)
            {
                return NotFound();
            }
           return Ok(review);
        }

        [HttpDelete]
        [Route("deleteReview/{id:guid}")]
        public IActionResult DeleteReview(Guid id)
        {
            Task<string?> result = _movieRepository.DeleteReviewAsync(id);
            return result is null? NotFound() : Ok(result);
        }
    }
}
