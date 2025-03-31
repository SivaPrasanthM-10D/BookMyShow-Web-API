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
        public async Task<IActionResult> GetAllMovies()
        {
            List<Movie>? movies = await _movieRepository.GetAllMoviesAsync();
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetMovieById(Guid id)
        {
            Movie? movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpGet]
        [Route("{title}")]
        public async Task<IActionResult> GetMovieByTitle(string title)
        {
            List<Movie>? movies = await _movieRepository.GetMovieByTitleAsync(title);
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieDto addmoviedto)
        {
            Movie? result = await _movieRepository.AddMovieAsync(addmoviedto);
            if (result == null)
            {
                return BadRequest("Invalid AdminId");
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateMovie(Guid id, UpdateMovieDto updatemoviedto)
        {
            Movie? movie = await _movieRepository.UpdateMovieAsync(id, updatemoviedto);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            string? result = await _movieRepository.DeleteMovieAsync(id);
            if (result == null)
            {
                return NotFound("Movie not found.");
            }
            return Ok(result);
        }

        [HttpPatch]
        [Route("{id:guid}")]
        public async Task<IActionResult> EditMovie(Guid id, [FromBody] JsonPatchDocument<Movie> patchmovie)
        {
            if (patchmovie == null)
            {
                return BadRequest();
            }
            Movie? movie = await _movieRepository.EditMovieAsync(id, patchmovie);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpGet]
        [Route("reviews/{movieId:guid}")]
        public async Task<IActionResult> GetReviewsByMovieId(Guid movieId)
        {
            List<ReviewResponse>? review = await _movieRepository.GetReviewsByMovieIdAsync(movieId);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        [HttpDelete]
        [Route("deleteReview/{id:guid}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            string? result = await _movieRepository.DeleteReviewAsync(id);
            return result == null ? NotFound() : Ok(result);
        }
    }
}
