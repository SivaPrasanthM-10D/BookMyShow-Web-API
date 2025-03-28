using BookMyShow.Data.Entities;
using BookMyShow.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Interfaces
{
    public interface IMoviesController
    {
        IActionResult GetAllMovies();
        IActionResult GetMovieById(Guid id);
        IActionResult GetMovieByTitle(string title);
        IActionResult AddMovie(AddMovieDto addmoviedto);
        IActionResult UpdateMovie(Guid id, UpdateMovieDto updatemoviedto);
        IActionResult DeleteMovie(Guid id);
        IActionResult EditMovie(Guid id, JsonPatchDocument<Movie> patchmovie);
        IActionResult GetReviewsByMovieId(Guid movieId);
        IActionResult DeleteReview(Guid id);
    }
}