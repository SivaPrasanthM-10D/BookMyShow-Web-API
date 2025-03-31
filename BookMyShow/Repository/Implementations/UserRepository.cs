using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Models;
using BookMyShow.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly BookMyShowDbContext dbContext;

        public UserRepository(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<User?> AddUsersAsync(AddUserDto adduserdto)
        {
            try
            {
                User user = new()
                {
                    UserName = adduserdto.UserName,
                    Password = adduserdto.Password,
                    Email = adduserdto.Email,
                    PhoneNumber = adduserdto.PhoneNumber,
                    Role = adduserdto.Role
                };
                await dbContext.Users.AddAsync(user);
                int result = await dbContext.SaveChangesAsync();

                if (result == 0)
                {
                    return null;
                }

                if (user.Role.Equals("TheatreOwner"))
                {
                    TheatreOwner theatreowner = new()
                    {
                        TheatreOwnerId = user.UserId,
                        TheatreOwnerName = user.UserName
                    };
                    await dbContext.TheatreOwners.AddAsync(theatreowner);
                    await dbContext.SaveChangesAsync();
                }
                else if (user.Role.Equals("Customer"))
                {
                    Customer customer = new()
                    {
                        CustomerId = user.UserId,
                        CustomerName = user.UserName
                    };
                    await dbContext.Customers.AddAsync(customer);
                    await dbContext.SaveChangesAsync();
                }
                else if (user.Role.Equals("Admin"))
                {
                    Admin admin = new()
                    {
                        AdminId = user.UserId,
                        AdminName = user.UserName
                    };
                    await dbContext.Admins.AddAsync(admin);
                    await dbContext.SaveChangesAsync();
                }
                return user;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return null;
            }
        }

        public async Task<string?> DeleteUserAsync(Guid userid)
        {
            try
            {
                var user = await dbContext.Users.FindAsync(userid);
                if (user == null)
                {
                    return null;
                }

                dbContext.Users.Remove(user);

                if (user.Role.Equals("TheatreOwner"))
                {
                    var theatreOwner = await dbContext.TheatreOwners.FindAsync(userid);
                    if (theatreOwner != null)
                    {
                        var theatres = await dbContext.Theatres.Where(t => t.TheatreOwnerId == theatreOwner.TheatreOwnerId).ToListAsync();
                        foreach (var theatre in theatres)
                        {
                            var screens = await dbContext.Screens.Where(s => s.TheatreId == theatre.TheatreId).ToListAsync();
                            foreach (var screen in screens)
                            {
                                var shows = await dbContext.Shows.Where(sh => sh.ScreenId == screen.ScreenId).ToListAsync();
                                foreach (var show in shows)
                                {
                                    var tickets = await dbContext.Tickets.Where(t => t.ShowId == show.ShowId).ToListAsync();
                                    foreach (var ticket in tickets)
                                    {
                                        dbContext.Tickets.Remove(ticket);
                                    }
                                    dbContext.Shows.Remove(show);
                                }
                                dbContext.Screens.Remove(screen);
                            }
                            dbContext.Theatres.Remove(theatre);
                        }
                        dbContext.TheatreOwners.Remove(theatreOwner);
                    }
                }
                else if (user.Role.Equals("Customer"))
                {
                    var customer = await dbContext.Customers.FindAsync(userid);
                    if (customer != null)
                    {
                        var tickets = await dbContext.Tickets.Where(t => t.CustomerId == customer.CustomerId).ToListAsync();
                        foreach (var ticket in tickets)
                        {
                            dbContext.Tickets.Remove(ticket);
                        }
                        dbContext.Customers.Remove(customer);
                    }
                }
                else if (user.Role.Equals("Admin"))
                {
                    var admin = await dbContext.Admins.FindAsync(userid);
                    if (admin != null)
                    {
                        var movies = await dbContext.Movies.Where(m => m.AdminId == admin.AdminId).ToListAsync();
                        foreach (var movie in movies)
                        {
                            dbContext.Movies.Remove(movie);
                        }
                        dbContext.Admins.Remove(admin);
                    }
                }

                await dbContext.SaveChangesAsync();
                return "User deleted successfully";
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return null;
            }
        }
    }
}
