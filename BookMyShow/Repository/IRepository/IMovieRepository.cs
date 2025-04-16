using BookMyShow.Data.Entities;
using BookMyShow.Models.CommonDTOs;
using BookMyShow.Models.MovieDTOs;
using BookMyShow.Models.TheatreDTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace BookMyShow.Repository.IRepository
{
    public interface IMovieRepository
    {
        Task<ServiceResult<List<Movie>?>> GetAllMoviesAsync();
        Task<ServiceResult<Movie?>> GetMovieByIdAsync(Guid id);
        Task<ServiceResult<List<Movie>?>> GetMovieByTitleAsync(string title);
        Task<ServiceResult<Movie?>> AddMovieAsync(AddMovieDto addmoviedto);
        Task<ServiceResult<Movie?>> UpdateMovieAsync(Guid id, UpdateMovieDto updatemoviedto);
        Task<ServiceResult<string?>> DeleteMovieAsync(Guid id);
        Task<ServiceResult<Movie?>> EditMovieAsync(Guid id, JsonPatchDocument<Movie> patchmovie);
    }
}