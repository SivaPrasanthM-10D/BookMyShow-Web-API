using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Exceptions;
using BookMyShow.Models.TheatreDTOs;
using BookMyShow.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static BookMyShow.Models.TheatreDTOs.AddScreenResponseDto;

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
            TheatreOwner? theatreOwner = await dbContext.TheatreOwners
                .Include(to => to.Theatre)
                .FirstOrDefaultAsync(to => to.TheatreOwnerId == ownerid);

            if (theatreOwner == null)
            {
                throw new TheatreOwnerNotFoundException("Theatre owner not found.");
            }

            return new TheatreOwnerSummaryDto
            {
                TheatreOwnerId = theatreOwner.TheatreOwnerId,
                TheatreOwnerName = theatreOwner.TheatreOwnerName,
                Theatre = theatreOwner.Theatre != null ? new TheatreDto
                {
                    TheatreId = theatreOwner.Theatre.TheatreId,
                    TheatreName = theatreOwner.Theatre.TheatreName,
                    TheatreOwnerId = theatreOwner.TheatreOwnerId,
                    City = theatreOwner.Theatre.City,
                    Street = theatreOwner.Theatre.Street
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
            TheatreOwner? theatreOwner = await dbContext.TheatreOwners.Include(to => to.Theatre).FirstOrDefaultAsync(to => to.TheatreOwnerId == ownerid);
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
            theatreOwner.Theatre = theatre;
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
                .Include(to => to.Theatre)
                .FirstOrDefaultAsync(to => to.TheatreOwnerId == ownerid);

            if (theatreOwner == null)
            {
                throw new TheatreOwnerNotFoundException("Theatre owner not found.");
            }

            if (theatreOwner.Theatre == null)
            {
                throw new TheatreNotFoundException("Theatre not found.");
            }

            dbContext.Theatres.Remove(theatreOwner.Theatre);
            theatreOwner.Theatre = null;
            await dbContext.SaveChangesAsync();

            return "Theatre deleted successfully";
        }
    }
}