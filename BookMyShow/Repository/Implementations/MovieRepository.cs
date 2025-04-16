using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Exceptions;
using BookMyShow.Models.CommonDTOs;
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

        public MovieRepository(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ServiceResult<List<Movie>?>> GetAllMoviesAsync()
        {
            try
            {
                List<Movie> movies = await dbContext.Movies.ToListAsync();
                if (movies.Count == 0)
                {
                    throw new NotFoundException("No movies found.");

                }
                return new ServiceResult<List<Movie>?> { Success = true, StatusCode = 200, Message = "Ok", Data = movies };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResult<List<Movie>?> { Success = false, StatusCode = 404, Message = ex.Message, Data = null };
            }
        }

        public async Task<ServiceResult<Movie?>> GetMovieByIdAsync(Guid id)
        {
            try
            {
                Movie? movie = await dbContext.Movies.FindAsync(id);
                if (movie is null)
                {
                    throw new NotFoundException("Movie not found.");
                }
                return new ServiceResult<Movie?> { Success = true, StatusCode = 200, Message = "Ok", Data = movie };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResult<Movie?> { Success = false, StatusCode = 404, Message = ex.Message, Data = null };
            }
        }

        public async Task<ServiceResult<List<Movie>?>> GetMovieByTitleAsync(string title)
        {
            try
            {
                List<Movie>? movies = await dbContext.Movies.Where(movie => movie.Title == title).ToListAsync();
                if (movies.Count == 0)
                {
                    throw new NotFoundException("No movies found with the specified title.");
                }
                return new ServiceResult<List<Movie>?> { Success = true, StatusCode = 200, Message = "Ok", Data = movies };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResult<List<Movie>?> { Success = false, StatusCode = 404, Message = ex.Message, Data = null };
            }
        }

        public async Task<ServiceResult<Movie?>> AddMovieAsync(AddMovieDto addmoviedto)
        {
            try
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
                return new ServiceResult<Movie?> { Success = true, StatusCode = 200, Message = "Ok", Data = movie };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResult<Movie?> { Success = false, StatusCode = 404, Message = ex.Message, Data = null };
            }
        }

        public async Task<ServiceResult<Movie?>> UpdateMovieAsync(Guid id, UpdateMovieDto updatemoviedto)
        {
            try
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
                return new ServiceResult<Movie?> { Success = true, StatusCode = 200, Message = "Ok", Data = movie };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResult<Movie?> { Success = false, StatusCode = 404, Message = ex.Message, Data = null };
            }
        }

        public async Task<ServiceResult<string?>> DeleteMovieAsync(Guid id)
        {
            try
            {
                Movie? movie = await dbContext.Movies.FindAsync(id);
                if (movie is null)
                {
                    throw new NotFoundException("Movie not found.");
                }
                dbContext.Movies.Remove(movie);
                await dbContext.SaveChangesAsync();
                return new ServiceResult<string?> { Success = true, StatusCode = 200, Message = "Success", Data = null };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResult<string?> { Success = false, StatusCode = 404, Message = ex.Message, Data = null };
            }
        }

        public async Task<ServiceResult<Movie?>> EditMovieAsync(Guid id, JsonPatchDocument<Movie> patchmovie)
        {
            try
            {
                Movie? movie = await dbContext.Movies.FindAsync(id);
                if (movie is null)
                {
                    throw new NotFoundException("Movie not found.");
                }
                patchmovie.ApplyTo(movie);
                await dbContext.SaveChangesAsync();
                return new ServiceResult<Movie?> { Success = true, StatusCode = 200, Message = "Success", Data = movie };
            }
            catch (NotFoundException ex)
            {
                return new ServiceResult<Movie?> { Success = false, StatusCode = 404, Message = ex.Message, Data = null };
            }
        }
    }
}