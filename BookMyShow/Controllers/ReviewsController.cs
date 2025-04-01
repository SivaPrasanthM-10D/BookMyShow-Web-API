using BookMyShow.Exceptions;
using BookMyShow.Models.ReviewDTOs;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Adds a new review.
        /// </summary>
        /// <param name="addReviewDto">The review details to add.</param>
        /// <returns>The added review.</returns>
        /// <exception cref="BadRequestException">Thrown when the review data is invalid.</exception>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> AddReview(AddReviewDto addReviewDto)
        {
            try
            {
                ReviewResponse? result = await _reviewRepository.AddReviewAsync(addReviewDto);
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="id">The ID of the review to update.</param>
        /// <param name="updateReviewDto">The updated review details.</param>
        /// <returns>The updated review.</returns>
        /// <exception cref="NotFoundException">Thrown when the review is not found.</exception>
        [Authorize(Roles = "Customer")]
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateReview(Guid id, UpdateReviewDto updateReviewDto)
        {
            try
            {
                ReviewResponse? result = await _reviewRepository.UpdateReviewAsync(id, updateReviewDto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves reviews for a specific movie.
        /// </summary>
        /// <param name="movieId">The ID of the movie to retrieve reviews for.</param>
        /// <returns>A list of reviews for the specified movie.</returns>
        /// <exception cref="NotFoundException">Thrown when the movie or reviews are not found.</exception>
        [HttpGet]
        [Route("{movieId:guid}")]
        public async Task<IActionResult> GetReviewsByMovieId(Guid movieId)
        {
            try
            {
                List<ReviewResponse>? reviews = await _reviewRepository.GetReviewsByMovieIdAsync(movieId);
                return Ok(reviews);
            }
            catch (NotFoundException)
            {
                return NotFound("No reviews found for the specified movie.");
            }
        }

        /// <summary>
        /// Deletes a review.
        /// </summary>
        /// <param name="reviewid">The ID of the review to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <exception cref="NotFoundException">Thrown when the review is not found.</exception>
        [Authorize(Roles = "Customer,Admin")]
        [HttpDelete]
        [Route("{reviewid:guid}")]
        public async Task<IActionResult> DeleteReview(Guid reviewid)
        {
            try
            {
                string? review = await _reviewRepository.DeleteReviewAsync(reviewid);
                return Ok(review);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}