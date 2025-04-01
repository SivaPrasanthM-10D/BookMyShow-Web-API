using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Exceptions;
using BookMyShow.Models.MovieDTOs;
using BookMyShow.Models.TheatreDTOs;
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

        /// <summary>
        /// Retrieves all movies from the database.
        /// </summary>
        /// <returns>A list of movies.</returns>
        /// <exception cref="NotFoundException">Thrown when no movies are found.</exception>
        public async Task<List<Movie>?> GetAllMoviesAsync()
        {
            List<Movie> movies = await dbContext.Movies.ToListAsync();
            if (movies.Count == 0)
            {
                throw new NotFoundException("No movies found.");
            }
            return movies;
        }

        /// <summary>
        /// Retrieves a movie by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        /// <returns>The movie with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when the movie is not found.</exception>
        public async Task<Movie?> GetMovieByIdAsync(Guid id)
        {
            Movie? movie = await dbContext.Movies.FindAsync(id);
            if (movie is null)
            {
                throw new NotFoundException("Movie not found.");
            }
            return movie;
        }

        /// <summary>
        /// Retrieves movies by their title.
        /// </summary>
        /// <param name="title">The title of the movies to retrieve.</param>
        /// <returns>A list of movies with the specified title.</returns>
        /// <exception cref="NotFoundException">Thrown when no movies with the specified title are found.</exception>
        public async Task<List<Movie>?> GetMovieByTitleAsync(string title)
        {
            List<Movie> movies = await dbContext.Movies.Where(movie => movie.Title == title).ToListAsync();
            if (movies.Count == 0)
            {
                throw new NotFoundException("No movies found with the specified title.");
            }
            return movies;
        }

        /// <summary>
        /// Adds a new movie to the database.
        /// </summary>
        /// <param name="addmoviedto">The movie details to add.</param>
        /// <returns>The added movie.</returns>
        /// <exception cref="NotFoundException">Thrown when the admin is not found.</exception>
        public async Task<Movie?> AddMovieAsync(AddMovieDto addmoviedto)
        {
            var admin = await dbContext.Admins.FindAsync(addmoviedto.AdminId);
            if (admin == null)
            {
                throw new NotFoundException("Admin not found.");
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

        /// <summary>
        /// Updates an existing movie in the database.
        /// </summary>
        /// <param name="id">The ID of the movie to update.</param>
        /// <param name="updatemoviedto">The updated movie details.</param>
        /// <returns>The updated movie.</returns>
        /// <exception cref="NotFoundException">Thrown when the movie is not found.</exception>
        public async Task<Movie?> UpdateMovieAsync(Guid id, UpdateMovieDto updatemoviedto)
        {
            Movie? movie = await dbContext.Movies.FindAsync(id);
            if (movie is null)
            {
                throw new NotFoundException("Movie not found.");
            }
            movie.Title = updatemoviedto.Title;
            movie.Genre = updatemoviedto.Genre;
            movie.Duration = updatemoviedto.Duration;
            movie.Rating = updatemoviedto.Rating;
            await dbContext.SaveChangesAsync();
            return movie;
        }

        /// <summary>
        /// Deletes a movie from the database.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <exception cref="NotFoundException">Thrown when the movie is not found.</exception>
        public async Task<string?> DeleteMovieAsync(Guid id)
        {
            Movie? movie = await dbContext.Movies.FindAsync(id);
            if (movie is null)
            {
                throw new NotFoundException("Movie not found.");
            }
            dbContext.Movies.Remove(movie);
            await dbContext.SaveChangesAsync();
            return "Movie deleted successfully";
        }

        /// <summary>
        /// Applies a JSON patch to an existing movie.
        /// </summary>
        /// <param name="id">The ID of the movie to patch.</param>
        /// <param name="patchmovie">The JSON patch document.</param>
        /// <returns>The patched movie.</returns>
        /// <exception cref="NotFoundException">Thrown when the movie is not found.</exception>
        public async Task<Movie?> EditMovieAsync(Guid id, JsonPatchDocument<Movie> patchmovie)
        {
            Movie? movie = await dbContext.Movies.FindAsync(id);
            if (movie is null)
            {
                throw new NotFoundException("Movie not found.");
            }
            patchmovie.ApplyTo(movie);
            await dbContext.SaveChangesAsync();
            return movie;
        }
    }
}
