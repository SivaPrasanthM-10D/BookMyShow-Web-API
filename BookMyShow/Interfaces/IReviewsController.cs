using BookMyShow.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Interfaces
{
    public interface IReviewsController
    {
        IActionResult AddReview(AddReviewDto addReviewDto);
        IActionResult UpdateReview(Guid id, UpdateReviewDto updateReviewDto);
        IActionResult GetReviewsByMovieId(Guid movieId);
        IActionResult DeleteReview(Guid id);
    }
}
