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
    public class ReviewsController : ControllerBase, IReviewsController
    {
        private readonly BookMyShowDbContext dbContext;

        public ReviewsController(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("addReview")]
        public IActionResult AddReview(AddReviewDto addReviewDto)
        {
            MovieReview review = new()
            {
                UserId = addReviewDto.UserId,
                MovieId = addReviewDto.MovieId,
                Rating = addReviewDto.Rating,
                Review = addReviewDto.Review
            };

            dbContext.MovieReviews.Add(review);
            dbContext.SaveChanges();

            review = dbContext.MovieReviews
                .Include(r => r.User)
                .Include(r => r.Movie)
                .FirstOrDefault(r => r.ReviewId == review.ReviewId)!;

            return Ok(new
            {
                review.ReviewId,
                review.UserId,
                review.MovieId,
                review.Rating,
                review.Review
            });
        }

        [HttpPut]
        [Route("updateReview/{id:guid}")]
        public IActionResult UpdateReview(Guid id, UpdateReviewDto updateReviewDto)
        {
            MovieReview? review = dbContext.MovieReviews.Find(id);
            if (review is null)
            {
                return NotFound();
            }
            review.Rating = updateReviewDto.Rating;
            review.Review = updateReviewDto.Review;
            dbContext.SaveChanges();
            return Ok(new
            {
                review.ReviewId,
                review.UserId,
                review.MovieId,
                review.Rating,
                review.Review
            });
        }

        [HttpGet]
        [Route("reviews/{movieId:guid}")]
        public IActionResult GetReviewsByMovieId(Guid movieId)
        {
            var reviews = dbContext.MovieReviews
                .Include(r => r.User)
                .Include(r => r.Movie)
                .Where(r => r.MovieId == movieId)
                .Select(review => new
                {
                    review.ReviewId,
                    review.UserId,
                    review.MovieId,
                    review.Rating,
                    review.Review
                })
                .ToList();
            return Ok(reviews);
        }

        [HttpDelete]
        [Route("deleteReview/{reviewid:guid}")]
        public IActionResult DeleteReview(Guid reviewid)
        {
            MovieReview? review = dbContext.MovieReviews.Find(reviewid);
            if (review is null)
            {
                return NotFound();
            }
            dbContext.MovieReviews.Remove(review);
            dbContext.SaveChanges();
            return Ok("Review deleted successfully");
        }
    }
}
