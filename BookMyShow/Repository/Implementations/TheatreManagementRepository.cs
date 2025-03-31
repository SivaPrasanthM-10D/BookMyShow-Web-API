using System.Globalization;
using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Models;
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

        public async Task<ScreenResponseDto?> AddScreenAsync(AddScreenDto addScreenDto)
        {
            Theatre? theatre = await dbContext.Theatres
                .Include(t => t.TheatreOwner)
                .Include(t => t.Screens)
                .FirstOrDefaultAsync(t => t.TheatreId == addScreenDto.TheatreId);
            if (theatre == null)
            {
                return null;
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
                .FirstOrDefaultAsync(s => s.ScreenId == screen.ScreenId)!;

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

        public async Task<string?> DeleteScreenAsync(Guid screenId)
        {
            var screen = await dbContext.Screens.FindAsync(screenId);
            if (screen == null)
            {
                return null;
            }

            dbContext.Screens.Remove(screen);
            await dbContext.SaveChangesAsync();
            return "Screen deleted successfully";
        }

        public async Task<ScreenResponseDto?> UpdateScreenAsync(Guid screenId, UpdateScreenDto updateScreenDto)
        {
            var screen = await dbContext.Screens.FindAsync(screenId);
            if (screen == null)
            {
                return null;
            }

            screen.ScreenNumber = updateScreenDto.ScreenNumber;
            await dbContext.SaveChangesAsync();

            screen = await dbContext.Screens
                .Include(s => s.Theatre)
                .Include(s => s.Shows)
                .FirstOrDefaultAsync(s => s.ScreenId == screen.ScreenId)!;

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

        public async Task<ShowResponseDto?> AddShowAsync(AddShowDto addShowDto)
        {
            var screen = await dbContext.Screens
                .Include(s => s.Theatre)
                .FirstOrDefaultAsync(s => s.ScreenId == addShowDto.ScreenId);

            if (screen == null)
            {
                return null;
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

        public async Task<string?> DeleteShowAsync(Guid showId)
        {
            var show = await dbContext.Shows.FindAsync(showId);
            if (show == null)
            {
                return null;
            }

            dbContext.Shows.Remove(show);
            await dbContext.SaveChangesAsync();
            return "Show deleted successfully.";
        }

        public async Task<ShowResponseDto?> UpdateShowAsync(Guid showId, UpdateShowDto updateShowDto)
        {
            var show = await dbContext.Shows.FindAsync(showId);
            if (show == null)
            {
                return null;
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
                .FirstOrDefaultAsync(s => s.ScreenId == show.ScreenId)!;
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

        public async Task<string?> RemoveTheatreofTheatreOwnerAsync(Guid ownerId)
        {
            var theatreOwner = await dbContext.TheatreOwners.FindAsync(ownerId);
            if (theatreOwner is null)
            {
                return null;
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