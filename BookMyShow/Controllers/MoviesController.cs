using BookMyShow.Data.Entities;
using BookMyShow.Models.CommonDTOs;
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
        /// <response code="200">Returns the list of movies</response>
        /// <response code="404">If no movies are found</response>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            ServiceResult<List<Movie>?> result = await _movieRepository.GetAllMoviesAsync();
            if (result.Success) return Ok(result);
            return NotFound(result);
        }

        /// <summary>
        /// Retrieves a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        /// <returns>The movie with the specified ID.</returns>
        /// <response code="200">Returns the movie with the specified ID</response>
        /// <response code="404">If the movie is not found</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetMovieById(Guid id)
        {
            ServiceResult<Movie?> result = await _movieRepository.GetMovieByIdAsync(id);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }

        /// <summary>
        /// Retrieves movies by their title.
        /// </summary>
        /// <param name="title">The title of the movies to retrieve.</param>
        /// <returns>A list of movies with the specified title.</returns>
        /// <response code="200">Returns the list of movies with the specified title</response>
        /// <response code="404">If no movies are found with the specified title</response>
        [Authorize(Roles = "TheatreOwner,Admin,Customer")]
        [HttpGet]
        [Route("{title}")]
        public async Task<IActionResult> GetMovieByTitleAsync(string title)
        {
            ServiceResult<List<Movie>?> result = await _movieRepository.GetMovieByTitleAsync(title);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }

        /// <summary>
        /// Adds a new movie.
        /// </summary>
        /// <param name="addmoviedto">The movie details to add.</param>
        /// <returns>The added movie.</returns>
        /// <response code="200">Returns the added movie</response>
        /// <response code="404">If the movie could not be added</response>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieDto addmoviedto)
        {
            ServiceResult<Movie?> result = await _movieRepository.AddMovieAsync(addmoviedto);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }

        /// <summary>
        /// Updates an existing movie.
        /// </summary>
        /// <param name="id">The ID of the movie to update.</param>
        /// <param name="updatemoviedto">The updated movie details.</param>
        /// <returns>The updated movie.</returns>
        /// <response code="200">Returns the updated movie</response>
        /// <response code="404">If the movie to update is not found</response>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateMovie(Guid id, UpdateMovieDto updatemoviedto)
        {
            ServiceResult<Movie?> result = await _movieRepository.UpdateMovieAsync(id, updatemoviedto);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }

        /// <summary>
        /// Deletes a movie.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <response code="200">Returns a message indicating the result of the deletion</response>
        /// <response code="404">If the movie to delete is not found</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            ServiceResult<string?> result = await _movieRepository.DeleteMovieAsync(id);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }

        /// <summary>
        /// Applies a JSON patch to an existing movie.
        /// </summary>
        /// <param name="id">The ID of the movie to patch.</param>
        /// <param name="patchmovie">The JSON patch document.</param>
        /// <returns>The patched movie.</returns>
        /// <response code="200">Returns the patched movie</response>
        /// <response code="400">If the JSON patch document is invalid</response>
        /// <response code="404">If the movie to patch is not found</response>
        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [Route("{id:guid}")]
        public async Task<IActionResult> EditMovie(Guid id, [FromBody] JsonPatchDocument<Movie> patchmovie)
        {
            if (patchmovie == null)
            {
                return BadRequest(new ServiceResult<Movie?> { Success = false, StatusCode = 400, Message = "Invalid JSON patch document.", Data = null });
            }
            ServiceResult<Movie?> result = await _movieRepository.EditMovieAsync(id, patchmovie);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }
    }
}
