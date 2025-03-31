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
        public IActionResult AddReview(AddReviewDto addReviewDto)
        {
            Task<ReviewResponse?> result = _reviewRepository.AddReviewAsync(addReviewDto);
            if(result is null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("updateReview/{id:guid}")]
        public IActionResult UpdateReview(Guid id, UpdateReviewDto updateReviewDto)
        {
            Task<ReviewResponse?> result = _reviewRepository.UpdateReviewAsync(id, updateReviewDto);
            if (result is null)
            {
                return NotFound("Review not found");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("reviews/{movieId:guid}")]
        public IActionResult GetReviewsByMovieId(Guid movieId)
        {
            Task<List<ReviewResponse>?> reviews = _reviewRepository.GetReviewsByMovieIdAsync(movieId);
            if(reviews is null)
            {
                return NotFound();
            }
            return Ok(reviews);
        }

        [HttpDelete]
        [Route("deleteReview/{reviewid:guid}")]
        public IActionResult DeleteReview(Guid reviewid)
        {
            Task<string?> review = _reviewRepository.DeleteReviewAsync(reviewid);
            if (review is null)
            {
                return NotFound();
            }
            return Ok(review);
        }
    }
}