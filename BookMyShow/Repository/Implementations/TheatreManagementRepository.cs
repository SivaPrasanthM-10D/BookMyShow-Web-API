using System.Globalization;
using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Exceptions;
using BookMyShow.Models.TheatreDTOs;
using BookMyShow.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repository.Implementations
{
    public class TheatreManagementRepository : ITheatreManagementRepository
    {
        private readonly BookMyShowDbContext dbContext;

        public TheatreManagementRepository(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all theatres from the database.
        /// </summary>
        /// <returns>A list of theatres.</returns>
        public async Task<List<TheatreResponseDto>> GetAllTheatresAsync()
        {
            List<TheatreResponseDto> theatres = await dbContext.Theatres
                .Include(t => t.TheatreOwner)
                .Include(t => t.Screens)
                .Select(t => new TheatreResponseDto
                {
                    TheatreId = t.TheatreId,
                    TheatreName = t.TheatreName,
                    TheatreOwnerId = t.TheatreOwnerId,
                    TheatreOwnerName = t.TheatreOwner.TheatreOwnerName,
                    ScreenCount = t.Screens.Count,
                    Street = t.Street,
                    City = t.City
                })
                .ToListAsync();
            return theatres;
        }

        /// <summary>
        /// Deletes a theatre by the theatre's ID.
        /// </summary>
        /// <param name="theatreid">The ID of the theatre.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <exception cref="TheatreOwnerNotFoundException">Thrown when the theatre owner is not found.</exception>
        /// <exception cref="TheatreNotFoundException">Thrown when the theatre is not found.</exception>
        public async Task<string?> DeleteTheatreAsync(Guid theatreid)
        {
            Theatre? theatre = await dbContext.Theatres.FindAsync(theatreid);


            if (theatre == null)
            {
                throw new TheatreNotFoundException("Theatre not found.");
            }

            dbContext.Theatres.Remove(theatre);
            await dbContext.SaveChangesAsync();

            return "Theatre deleted successfully";
        }

        /// <summary>
        /// Retrieves all screens for a specific theatre.
        /// </summary>
        /// <param name="theatreid">The ID of the theatre to retrieve screens for.</param>
        /// <returns>A list of screens for the specified theatre.</returns>
        public async Task<List<ScreenResponseDto>> GetAllScreensAsync(Guid theatreid)
        {
            List<ScreenResponseDto> screens = await dbContext.Screens
                .Include(s => s.Theatre)
                    .ThenInclude(t => t.TheatreOwner)
                .Include(s => s.Shows)
                .Where(s => s.TheatreId == theatreid)
                .Select(s => new ScreenResponseDto
                {
                    ScreenId = s.ScreenId,
                    ScreenNumber = s.ScreenNumber,
                    TheatreId = s.TheatreId,
                    TheatreName = s.Theatre.TheatreName,
                    TotalShows = s.Shows.Count
                })
                .ToListAsync();
            return screens;
        }

        /// <summary>
        /// Retrieves all shows for a specific screen.
        /// </summary>
        /// <param name="screenid">The ID of the screen to retrieve shows for.</param>
        /// <returns>A list of shows for the specified screen.</returns>
        public async Task<List<ShowResponseDto>> GetAllShowsAsync(Guid screenid)
        {
            List<ShowResponseDto> shows = await dbContext.Shows
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Theatre)
                        .ThenInclude(t => t.TheatreOwner)
                    .Where(s => s.ScreenId == screenid)
                .Select(s => new ShowResponseDto
                {
                    ShowId = s.ShowId,
                    ScreenId = s.ScreenId,
                    ScreenNumber = s.Screen.ScreenNumber,
                    TheatreId = s.Screen.TheatreId,
                    TheatreName = s.Screen.Theatre.TheatreName,
                    MovieId = s.MovieId,
                    ShowTime = DateTime.Today.Add(s.ShowTime).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                    ShowDate = s.ShowDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    AvailableSeats = s.AvailableSeats,
                    TicketPrice = s.TicketPrice
                })
                .ToListAsync();
            return shows;
        }

        /// <summary>
        /// Adds a new screen to a theatre.
        /// </summary>
        /// <param name="addScreenDto">The screen details to add.</param>
        /// <returns>The added screen.</returns>
        /// <exception cref="TheatreNotFoundException">Thrown when the theatre is not found.</exception>
        public async Task<ScreenResponseDto?> AddScreenAsync(AddScreenDto addScreenDto)
        {
            Theatre? theatre = await dbContext.Theatres
                .Include(t => t.TheatreOwner)
                .Include(t => t.Screens)
                .FirstOrDefaultAsync(t => t.TheatreId == addScreenDto.TheatreId);
            if (theatre == null)
            {
                throw new TheatreNotFoundException("Theatre not found.");
            }

            var screen = new Screen
            {
                ScreenNumber = addScreenDto.ScreenNumber,
                TheatreId = addScreenDto.TheatreId
            };

            await dbContext.Screens.AddAsync(screen);
            await dbContext.SaveChangesAsync();

            screen = await dbContext.Screens
                .Include(s => s.Theatre)
                .Include(s => s.Shows)
                .FirstOrDefaultAsync(s => s.ScreenId == screen.ScreenId)
                ?? throw new ScreenNotFoundException("Screen not found.");

            ScreenResponseDto response = new()
            {
                ScreenId = screen.ScreenId,
                ScreenNumber = screen.ScreenNumber,
                TheatreId = screen.Theatre.TheatreId,
                TheatreName = screen.Theatre.TheatreName,
                TotalShows = screen.Shows.Count
            };
            return response;
        }

        /// <summary>
        /// Deletes a screen from the database.
        /// </summary>
        /// <param name="screenId">The ID of the screen to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <exception cref="ScreenNotFoundException">Thrown when the screen is not found.</exception>
        public async Task<string?> DeleteScreenAsync(Guid screenId)
        {
            var screen = await dbContext.Screens.FindAsync(screenId);
            if (screen == null)
            {
                throw new ScreenNotFoundException("Screen not found.");
            }

            dbContext.Screens.Remove(screen);
            await dbContext.SaveChangesAsync();
            return "Screen deleted successfully";
        }

        /// <summary>
        /// Updates an existing screen in the database.
        /// </summary>
        /// <param name="screenId">The ID of the screen to update.</param>
        /// <param name="updateScreenDto">The updated screen details.</param>
        /// <returns>The updated screen.</returns>
        /// <exception cref="ScreenNotFoundException">Thrown when the screen is not found.</exception>
        public async Task<ScreenResponseDto?> UpdateScreenAsync(Guid screenId, UpdateScreenDto updateScreenDto)
        {
            var screen = await dbContext.Screens.FindAsync(screenId);
            if (screen == null)
            {
                throw new ScreenNotFoundException("Screen not found.");
            }

            screen.ScreenNumber = updateScreenDto.ScreenNumber;
            await dbContext.SaveChangesAsync();

            screen = await dbContext.Screens
                .Include(s => s.Theatre)
                .Include(s => s.Shows)
                .FirstOrDefaultAsync(s => s.ScreenId == screen.ScreenId)
                ?? throw new ScreenNotFoundException("Screen not found.");

            ScreenResponseDto response = new()
            {
                ScreenId = screen.ScreenId,
                ScreenNumber = screen.ScreenNumber,
                TheatreId = screen.Theatre.TheatreId,
                TheatreName = screen.Theatre.TheatreName,
                TotalShows = screen.Shows.Count
            };
            return response;
        }

        /// <summary>
        /// Adds a new show to a screen.
        /// </summary>
        /// <param name="addShowDto">The show details to add.</param>
        /// <returns>The added show.</returns>
        /// <exception cref="ScreenNotFoundException">Thrown when the screen is not found.</exception>
        public async Task<ShowResponseDto?> AddShowAsync(AddShowDto addShowDto)
        {
            var screen = await dbContext.Screens
                .Include(s => s.Theatre)
                .FirstOrDefaultAsync(s => s.ScreenId == addShowDto.ScreenId);

            if (screen == null)
            {
                throw new ScreenNotFoundException("Screen not found.");
            }

            List<int> AvailableSeats = new List<int>();
            for (int i = 1; i <= addShowDto.TotalSeats; i++)
                AvailableSeats.Add(i);

            var show = new Show
            {
                ScreenId = addShowDto.ScreenId,
                MovieId = addShowDto.MovieId,
                ShowTime = DateTime.ParseExact(addShowDto.ShowTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay,
                ShowDate = DateTime.ParseExact(addShowDto.ShowDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                AvailableSeats = AvailableSeats,
                TicketPrice = addShowDto.TicketPrice
            };

            await dbContext.Shows.AddAsync(show);
            await dbContext.SaveChangesAsync();
            return new ShowResponseDto
            {
                ShowId = show.ShowId,
                ScreenId = show.ScreenId,
                ScreenNumber = screen.ScreenNumber,
                TheatreId = screen.TheatreId,
                TheatreName = screen.Theatre.TheatreName,
                MovieId = show.MovieId,
                ShowTime = DateTime.Today.Add(show.ShowTime).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                ShowDate = show.ShowDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                AvailableSeats = show.AvailableSeats,
                TicketPrice = show.TicketPrice
            };
        }

        /// <summary>
        /// Deletes a show from the database.
        /// </summary>
        /// <param name="showId">The ID of the show to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <exception cref="ShowNotFoundException">Thrown when the show is not found.</exception>
        public async Task<string?> DeleteShowAsync(Guid showId)
        {
            var show = await dbContext.Shows.FindAsync(showId);
            if (show == null)
            {
                throw new ShowNotFoundException("Show not found.");
            }

            dbContext.Shows.Remove(show);
            await dbContext.SaveChangesAsync();
            return "Show deleted successfully.";
        }

        /// <summary>
        /// Updates an existing show in the database.
        /// </summary>
        /// <param name="showId">The ID of the show to update.</param>
        /// <param name="updateShowDto">The updated show details.</param>
        /// <returns>The updated show.</returns>
        /// <exception cref="ShowNotFoundException">Thrown when the show is not found.</exception>
        public async Task<ShowResponseDto?> UpdateShowAsync(Guid showId, UpdateShowDto updateShowDto)
        {
            var show = await dbContext.Shows.FindAsync(showId);
            if (show == null)
            {
                throw new ShowNotFoundException("Show not found.");
            }

            show.MovieId = updateShowDto.MovieId;
            show.ShowTime = TimeSpan.ParseExact(updateShowDto.ShowTime, @"hh\:mm tt", CultureInfo.InvariantCulture);
            show.ShowDate = DateTime.ParseExact(updateShowDto.ShowDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            show.AvailableSeats = updateShowDto.AvailableSeats;
            show.TicketPrice = updateShowDto.TicketPrice;
            await dbContext.SaveChangesAsync();

            Screen screen = await dbContext.Screens
                .Include(s => s.Theatre)
                .Include(s => s.Shows)
                .FirstOrDefaultAsync(s => s.ScreenId == show.ScreenId)
                ?? throw new ScreenNotFoundException("Screen not found.");
            return new ShowResponseDto
            {
                ShowId = show.ShowId,
                ScreenId = show.ScreenId,
                ScreenNumber = screen.ScreenNumber,
                TheatreId = screen.TheatreId,
                TheatreName = screen.Theatre.TheatreName,
                MovieId = show.MovieId,
                ShowTime = DateTime.Today.Add(show.ShowTime).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                ShowDate = show.ShowDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                AvailableSeats = show.AvailableSeats,
                TicketPrice = show.TicketPrice
            };
        }

        /// <summary>
        /// Removes a theatre from a theatre owner.
        /// </summary>
        /// <param name="ownerId">The ID of the theatre owner.</param>
        /// <returns>A message indicating the result of the removal.</returns>
        /// <exception cref="TheatreNotFoundException">Thrown when the theatre owner is not found.</exception>
        public async Task<string?> RemoveTheatreofTheatreOwnerAsync(Guid ownerId)
        {
            var theatreOwner = await dbContext.TheatreOwners.FindAsync(ownerId);
            if (theatreOwner is null)
            {
                throw new TheatreNotFoundException("Theatre Owner not found.");
            }
            var theatreids = dbContext.Theatres.Where(th => th.TheatreOwnerId == ownerId);
            foreach (var theatre in theatreids)
            {
                dbContext.Theatres.Remove(theatre);
            }
            await dbContext.SaveChangesAsync();
            return $"Successfully removed from {theatreOwner.TheatreOwnerName}.";
        }
    }
}