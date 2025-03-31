using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Models;
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

        public async Task<ReviewResponse?> AddReviewAsync(AddReviewDto addReviewDto)
        {
            try
            {
                if (addReviewDto is null)
                {
                    return null;
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
                    .FirstOrDefaultAsync(r => r.ReviewId == review.ReviewId)!;

                return new ReviewResponse
                {
                    ReviewId = review.ReviewId,
                    UserId = review.UserId,
                    MovieId = review.MovieId,
                    Rating = review.Rating,
                    Review = review.Review
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ReviewResponse?> UpdateReviewAsync(Guid id, UpdateReviewDto updateReviewDto)
        {
            try
            {
                MovieReview? review = await dbContext.MovieReviews.FindAsync(id);
                if (review is null)
                {
                    return null;
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
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ReviewResponse>?> GetReviewsByMovieIdAsync(Guid movieId)
        {
            try
            {
                if (await dbContext.Movies.FindAsync(movieId) is null)
                {
                    return null;
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
                if (reviews is null)
                {
                    return null;
                }
                return reviews;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return null;
            }
        }

        public async Task<string?> DeleteReviewAsync(Guid reviewid)
        {
            try
            {
                MovieReview? review = await dbContext.MovieReviews.FindAsync(reviewid);
                if (review is null)
                {
                    return null;
                }
                dbContext.MovieReviews.Remove(review);
                await dbContext.SaveChangesAsync();
                return "Review deleted successfully";
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return null;
            }
        }
    }
}