using BookMyShow.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Interfaces
{
    public interface ITheatreOwnersController
    {
        IActionResult GetMovieList();
        IActionResult GetTheatreOwners();
        IActionResult GetTheatre(Guid ownerid);
        IActionResult AddTheatreToTheatreOwner(Guid id, AddTheatreDto addtheatredto);
        IActionResult GetReviewsByMovieId(Guid movieId);
    }
}
