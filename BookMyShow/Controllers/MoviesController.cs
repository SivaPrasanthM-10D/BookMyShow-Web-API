using BookMyShow.Data.Entities;
using BookMyShow.Exceptions;
using BookMyShow.Models.MovieDTOs;
using BookMyShow.Models.TheatreDTOs;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Retrieves all movies.
        /// </summary>
        /// <returns>A list of movies.</returns>
        [Authorize(Roles = "TheatreOwner,Admin,Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            try
            {
                List<Movie>? movies = await _movieRepository.GetAllMoviesAsync();
                return Ok(movies);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        /// <returns>The movie with the specified ID.</returns>
        [Authorize(Roles = "TheatreOwner,Admin")]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetMovieById(Guid id)
        {
            try
            {
                Movie? movie = await _movieRepository.GetMovieByIdAsync(id);
                return Ok(movie);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves movies by their title.
        /// </summary>
        /// <param name="title">The title of the movies to retrieve.</param>
        /// <returns>A list of movies with the specified title.</returns>
        [Authorize(Roles = "TheatreOwner,Admin,Customer")]
        [HttpGet]
        [Route("{title}")]
        public async Task<IActionResult> GetMovieByTitle(string title)
        {
            try
            {
                List<Movie>? movies = await _movieRepository.GetMovieByTitleAsync(title);
                return Ok(movies);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new movie.
        /// </summary>
        /// <param name="addmoviedto">The movie details to add.</param>
        /// <returns>The added movie.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieDto addmoviedto)
        {
            try
            {
                Movie? result = await _movieRepository.AddMovieAsync(addmoviedto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing movie.
        /// </summary>
        /// <param name="id">The ID of the movie to update.</param>
        /// <param name="updatemoviedto">The updated movie details.</param>
        /// <returns>The updated movie.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateMovie(Guid id, UpdateMovieDto updatemoviedto)
        {
            try
            {
                Movie? movie = await _movieRepository.UpdateMovieAsync(id, updatemoviedto);
                return Ok(movie);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a movie.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            try
            {
                string? result = await _movieRepository.DeleteMovieAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Applies a JSON patch to an existing movie.
        /// </summary>
        /// <param name="id">The ID of the movie to patch.</param>
        /// <param name="patchmovie">The JSON patch document.</param>
        /// <returns>The patched movie.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [Route("{id:guid}")]
        public async Task<IActionResult> EditMovie(Guid id, [FromBody] JsonPatchDocument<Movie> patchmovie)
        {
            if (patchmovie == null)
            {
                return BadRequest();
            }
            try
            {
                Movie? movie = await _movieRepository.EditMovieAsync(id, patchmovie);
                return Ok(movie);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
