using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Models;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repository.Implementations
{
    public class MovieRepository : IMovieRepository
    {
        private readonly BookMyShowDbContext dbContext;
        private readonly IReviewRepository _reviewsRepository;

        public MovieRepository(BookMyShowDbContext dbContext, IReviewRepository reviewsRepository)
        {
            this.dbContext = dbContext;
            _reviewsRepository = reviewsRepository;
        }

        public async Task<List<Movie>?> GetAllMoviesAsync()
        {
            List<Movie> movies = await dbContext.Movies.ToListAsync();
            if (movies.Count == 0)
            {
                return null;
            }
            return movies;
        }

        public async Task<Movie?> GetMovieByIdAsync(Guid id)
        {
            Movie? movie = await dbContext.Movies.FindAsync(id);
            if (movie is null)
            {
                return null;
            }
            return movie;
        }

        public async Task<List<Movie>?> GetMovieByTitleAsync(string title)
        {
            List<Movie> movies = await dbContext.Movies.Where(movie => movie.Title == title).ToListAsync();
            if (movies.Count == 0)
            {
                return null;
            }
            return movies;
        }

        public async Task<Movie?> AddMovieAsync(AddMovieDto addmoviedto)
        {
            var admin = await dbContext.Admins.FindAsync(addmoviedto.AdminId);
            if (admin == null)
            {
                return null;
            }
            Movie movie = new Movie()
            {
                Title = addmoviedto.Title,
                Genre = addmoviedto.Genre,
                Duration = addmoviedto.Duration,
                Rating = addmoviedto.Rating,
                AdminId = addmoviedto.AdminId
            };

            await dbContext.Movies.AddAsync(movie);
            await dbContext.SaveChangesAsync();
            return movie;
        }

        public async Task<Movie?> UpdateMovieAsync(Guid id, UpdateMovieDto updatemoviedto)
        {
            Movie? movie = await dbContext.Movies.FindAsync(id);
            if (movie is null)
            {
                return null;
            }
            movie.Title = updatemoviedto.Title;
            movie.Genre = updatemoviedto.Genre;
            movie.Duration = updatemoviedto.Duration;
            movie.Rating = updatemoviedto.Rating;
            await dbContext.SaveChangesAsync();
            return movie;
        }

        public async Task<string?> DeleteMovieAsync(Guid id)
        {
            Movie? movie = await dbContext.Movies.FindAsync(id);
            if (movie is null)
            {
                return null;
            }
            dbContext.Movies.Remove(movie);
            await dbContext.SaveChangesAsync();
            return "Movie Deleted successfully";
        }

        public async Task<Movie?> EditMovieAsync(Guid id, JsonPatchDocument<Movie> patchmovie)
        {
            Movie? movie = await dbContext.Movies.FindAsync(id);
            if (movie is null)
            {
                return null;
            }
            patchmovie.ApplyTo(movie);
            await dbContext.SaveChangesAsync();
            return movie;
        }

        public async Task<List<ReviewResponse>?> GetReviewsByMovieIdAsync(Guid movieId)
        {
            return await _reviewsRepository.GetReviewsByMovieIdAsync(movieId);
        }

        public async Task<string?> DeleteReviewAsync(Guid id)
        {
            return await _reviewsRepository.DeleteReviewAsync(id);
        }
    }
}
