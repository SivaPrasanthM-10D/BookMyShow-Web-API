using BookMyShow.Models;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        public readonly IReviewRepository _reviewRepository;

        public ReviewsController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [HttpPost]
        [Route("addReview")]
        public async Task<IActionResult> AddReview(AddReviewDto addReviewDto)
        {
            ReviewResponse? result = await _reviewRepository.AddReviewAsync(addReviewDto);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("updateReview/{id:guid}")]
        public async Task<IActionResult> UpdateReview(Guid id, UpdateReviewDto updateReviewDto)
        {
            ReviewResponse? result = await _reviewRepository.UpdateReviewAsync(id, updateReviewDto);
            if (result == null)
            {
                return NotFound("Review not found");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("reviews/{movieId:guid}")]
        public async Task<IActionResult> GetReviewsByMovieId(Guid movieId)
        {
            List<ReviewResponse>? reviews = await _reviewRepository.GetReviewsByMovieIdAsync(movieId);
            if (reviews == null)
            {
                return NotFound();
            }
            return Ok(reviews);
        }

        [HttpDelete]
        [Route("deleteReview/{reviewid:guid}")]
        public async Task<IActionResult> DeleteReview(Guid reviewid)
        {
            string? review = await _reviewRepository.DeleteReviewAsync(reviewid);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }
    }
}