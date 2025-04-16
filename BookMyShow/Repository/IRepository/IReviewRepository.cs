using BookMyShow.Models.ReviewDTOs;

namespace BookMyShow.Repository.IRepository
{
    public interface IReviewRepository
    {
        Task<ReviewResponse?> AddReviewAsync(AddReviewDto addReviewDto);
        Task<ReviewResponse?> UpdateReviewAsync(Guid id, UpdateReviewDto updateReviewDto);
        Task<List<ReviewResponse>?> GetReviewsByMovieNameAsync(string movieName);
        Task<string?> DeleteReviewAsync(Guid reviewid);
    }
}
