using BookMyShow.Models;

namespace BookMyShow.Repository.IRepository
{
    public interface IReviewRepository
    {
        Task<ReviewResponse?> AddReviewAsync(AddReviewDto addReviewDto);
        Task<ReviewResponse?> UpdateReviewAsync(Guid id, UpdateReviewDto updateReviewDto);
        Task<List<ReviewResponse>?> GetReviewsByMovieIdAsync(Guid movieId);
        Task<string?> DeleteReviewAsync(Guid reviewid);
    }
}
