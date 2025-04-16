using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Exceptions;
using BookMyShow.Models.TheatreDTOs;
using BookMyShow.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repository.Implementations
{
    public class TheatreRepository : ITheatreRepository
    {
        private readonly BookMyShowDbContext dbContext;
        public TheatreRepository(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all theatre owners from the database.
        /// </summary>
        /// <returns>A list of theatre owners.</returns>
        public async Task<List<User>> GetTheatreOwnersAsync()
        {
            List<User> TheatreOwners = await dbContext.Users.Where(u => u.Role.Equals("TheatreOwner")).ToListAsync();
            return TheatreOwners;
        }

        /// <summary>
        /// Retrieves a theatre owner by their ID.
        /// </summary>
        /// <param name="ownerid">The ID of the theatre owner to retrieve.</param>
        /// <returns>The theatre owner with the specified ID.</returns>
        /// <exception cref="TheatreOwnerNotFoundException">Thrown when the theatre owner is not found.</exception>
        public async Task<TheatreOwnerSummaryDto?> GetTheatreAsync(Guid ownerid)
        {
            var theatreOwner = await dbContext.TheatreOwners
                .Include(to => to.Theatres)
                    .ThenInclude(t => t.Screens)
                        .ThenInclude(s => s.Shows)
                .FirstOrDefaultAsync(to => to.TheatreOwnerId == ownerid);

            if (theatreOwner == null)
                throw new TheatreOwnerNotFoundException("Theatre owner not found.");

            var theatre = theatreOwner.Theatres.FirstOrDefault();

            return new TheatreOwnerSummaryDto
            {
                TheatreOwnerId = theatreOwner.TheatreOwnerId,
                TheatreOwnerName = theatreOwner.TheatreOwnerName,
                Theatre = theatre != null ? new TheatreDto
                {
                    TheatreId = theatre.TheatreId,
                    TheatreName = theatre.TheatreName,
                    TheatreOwnerId = theatre.TheatreOwnerId,
                    City = theatre.City,
                    Street = theatre.Street,
                    Screens = theatre.Screens.Select(screen => new ScreenDto
                    {
                        ScreenId = screen.ScreenId,
                        ScreenNumber = screen.ScreenNumber,
                        TheatreId = screen.TheatreId,
                        Shows = screen.Shows.Select(show => new ShowDto
                        {
                            ShowId = show.ShowId,
                            MovieId = show.MovieId,
                            ShowTime = show.ShowTime,
                            ShowDate = show.ShowDate,
                            TicketPrice = show.TicketPrice,
                            AvailableSeats = show.AvailableSeats
                        }).ToList()
                    }).ToList()
                } : null
            };
        }

        /// <summary>
        /// Adds a new theatre to a theatre owner.
        /// </summary>
        /// <param name="ownerid">The ID of the theatre owner.</param>
        /// <param name="addtheatredto">The theatre details to add.</param>
        /// <returns>The added theatre.</returns>
        /// <exception cref="TheatreOwnerNotFoundException">Thrown when the theatre owner is not found.</exception>
        public async Task<TheatreDto?> AddTheatreToTheatreOwnerAsync(Guid ownerid, AddTheatreDto addtheatredto)
        {
            TheatreOwner? theatreOwner = await dbContext.TheatreOwners.Include(to => to.Theatres).FirstOrDefaultAsync(to => to.TheatreOwnerId == ownerid);
            if (theatreOwner == null)
            {
                throw new TheatreOwnerNotFoundException("Theatre owner not found.");
            }
            Theatre theatre = new()
            {
                TheatreName = addtheatredto.TheatreName,
                Street = addtheatredto.Street,
                City = addtheatredto.City,
                TheatreOwnerId = theatreOwner.TheatreOwnerId
            };
            await dbContext.Theatres.AddAsync(theatre);
            theatreOwner.Theatres.Add(theatre);
            await dbContext.SaveChangesAsync();
            return new TheatreDto
            {
                TheatreId = theatre.TheatreId,
                TheatreName = theatre.TheatreName,
                TheatreOwnerId = theatre.TheatreOwnerId,
                Street = theatre.Street,
                City = theatre.City
            };
        }

        /// <summary>
        /// Deletes a theatre by the theatre owner's ID.
        /// </summary>
        /// <param name="ownerid">The ID of the theatre owner.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <exception cref="TheatreOwnerNotFoundException">Thrown when the theatre owner is not found.</exception>
        /// <exception cref="TheatreNotFoundException">Thrown when the theatre is not found.</exception>
        public async Task<string?> DeleteTheatreAsync(Guid ownerid)
        {
            TheatreOwner? theatreOwner = await dbContext.TheatreOwners
                .Include(to => to.Theatres)
                .FirstOrDefaultAsync(to => to.TheatreOwnerId == ownerid);

            if (theatreOwner == null)
            {
                throw new TheatreOwnerNotFoundException("Theatre owner not found.");
            }

            Theatre? theatre = theatreOwner.Theatres.FirstOrDefault();
            if (theatre == null)
            {
                throw new TheatreNotFoundException("Theatre not found.");
            }

            dbContext.Theatres.Remove(theatre);
            theatreOwner.Theatres.Remove(theatre);
            await dbContext.SaveChangesAsync();

            return "Theatre deleted successfully";
        }
    }
}