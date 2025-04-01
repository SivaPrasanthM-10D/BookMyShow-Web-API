using BookMyShow.Data.Entities;
using BookMyShow.Models.MovieDTOs;
using BookMyShow.Models.ReviewDTOs;
using BookMyShow.Models.TheatreDTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Repository.IRepository
{
    public interface IMovieRepository
    {
        Task<List<Movie>?> GetAllMoviesAsync();
        Task<Movie?> GetMovieByIdAsync(Guid id);
        Task<List<Movie>?> GetMovieByTitleAsync(string title);
        Task<Movie?> AddMovieAsync(AddMovieDto addmoviedto);
        Task<Movie?> UpdateMovieAsync(Guid id, UpdateMovieDto updatemoviedto);
        Task<string?> DeleteMovieAsync(Guid id);
        Task<Movie?> EditMovieAsync(Guid id, JsonPatchDocument<Movie> patchmovie);
    }
}