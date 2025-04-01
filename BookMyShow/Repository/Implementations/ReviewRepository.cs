using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Exceptions;
using BookMyShow.Models.ReviewDTOs;
using BookMyShow.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repository.Implementations
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BookMyShowDbContext dbContext;

        public ReviewRepository(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Adds a new review to the database.
        /// </summary>
        /// <param name="addReviewDto">The review details to add.</param>
        /// <returns>The added review.</returns>
        /// <exception cref="InvalidReviewDataException">Thrown when the review data is invalid.</exception>
        /// <exception cref="ReviewNotFoundException">Thrown when the review is not found after adding.</exception>
        public async Task<ReviewResponse?> AddReviewAsync(AddReviewDto addReviewDto)
        {
            if (addReviewDto is null)
            {
                throw new InvalidReviewDataException("Invalid review data.");
            }

            MovieReview review = new()
            {
                UserId = addReviewDto.UserId,
                MovieId = addReviewDto.MovieId,
                Rating = addReviewDto.Rating,
                Review = addReviewDto.Review
            };

            await dbContext.MovieReviews.AddAsync(review);
            await dbContext.SaveChangesAsync();

            review = await dbContext.MovieReviews
                .Include(r => r.User)
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.ReviewId == review.ReviewId)
                ?? throw new ReviewNotFoundException("Review not found.");

            return new ReviewResponse
            {
                ReviewId = review.ReviewId,
                UserId = review.UserId,
                MovieId = review.MovieId,
                Rating = review.Rating,
                Review = review.Review
            };
        }

        /// <summary>
        /// Updates an existing review in the database.
        /// </summary>
        /// <param name="id">The ID of the review to update.</param>
        /// <param name="updateReviewDto">The updated review details.</param>
        /// <returns>The updated review.</returns>
        /// <exception cref="ReviewNotFoundException">Thrown when the review is not found.</exception>
        public async Task<ReviewResponse?> UpdateReviewAsync(Guid id, UpdateReviewDto updateReviewDto)
        {
            MovieReview? review = await dbContext.MovieReviews.FindAsync(id);
            if (review is null)
            {
                throw new ReviewNotFoundException("Review not found.");
            }

            review.Rating = updateReviewDto.Rating;
            review.Review = updateReviewDto.Review;
            await dbContext.SaveChangesAsync();

            return new ReviewResponse
            {
                ReviewId = review.ReviewId,
                UserId = review.UserId,
                MovieId = review.MovieId,
                Rating = review.Rating,
                Review = review.Review
            };
        }

        /// <summary>
        /// Retrieves reviews for a specific movie.
        /// </summary>
        /// <param name="movieId">The ID of the movie to retrieve reviews for.</param>
        /// <returns>A list of reviews for the specified movie.</returns>
        /// <exception cref="MovieNotFoundException">Thrown when the movie or reviews are not found.</exception>
        public async Task<List<ReviewResponse>?> GetReviewsByMovieIdAsync(Guid movieId)
        {
            if (await dbContext.Movies.FindAsync(movieId) is null)
            {
                throw new MovieNotFoundException("Movie not found.");
            }

            List<ReviewResponse> reviews = await dbContext.MovieReviews
                .Include(r => r.User)
                .Include(r => r.Movie)
                .Where(r => r.MovieId == movieId)
                .Select(review => new ReviewResponse
                {
                    ReviewId = review.ReviewId,
                    UserId = review.UserId,
                    MovieId = review.MovieId,
                    Rating = review.Rating,
                    Review = review.Review
                })
                .ToListAsync();

            if (reviews is null || !reviews.Any())
            {
                throw new MovieNotFoundException();
            }

            return reviews;
        }

        /// <summary>
        /// Deletes a review from the database.
        /// </summary>
        /// <param name="reviewid">The ID of the review to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <exception cref="ReviewNotFoundException">Thrown when the review is not found.</exception>
        public async Task<string?> DeleteReviewAsync(Guid reviewid)
        {
            MovieReview? review = await dbContext.MovieReviews.FindAsync(reviewid);
            if (review is null)
            {
                throw new ReviewNotFoundException("Review not found.");
            }

            dbContext.MovieReviews.Remove(review);
            await dbContext.SaveChangesAsync();
            return "Review deleted successfully";
        }
    }
}