using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Models;
using BookMyShow.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static BookMyShow.Models.AddScreenResponseDto;

namespace BookMyShow.Repository.Implementations
{
    public class TheatreRepository : ITheatreRepository
    {
        private readonly BookMyShowDbContext dbContext;

        public TheatreRepository(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<User>> GetTheatreOwnersAsync()
        {
            List<User> TheatreOwners = await dbContext.Users.Where(u => u.Role.Equals("TheatreOwner")).ToListAsync();
            return TheatreOwners;
        }

        public async Task<TheatreOwnerSummaryDto?> GetTheatreAsync(Guid ownerid)
        {
            TheatreOwner? theatreOwner = await dbContext.TheatreOwners.FindAsync(ownerid);

            if (theatreOwner == null)
            {
                return null;
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

        public async Task<TheatreDto?> AddTheatreToTheatreOwnerAsync(Guid ownerid, AddTheatreDto addtheatredto)
        {
            TheatreOwner? theatreOwner = await dbContext.TheatreOwners.Include(to => to.Theatre).FirstOrDefaultAsync(to => to.TheatreOwnerId == ownerid);
            if (theatreOwner == null)
            {
                return null;
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
    }
}
