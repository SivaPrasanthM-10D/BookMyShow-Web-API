using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BookMyShowDbContext dbContext;

        public UsersController(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(dbContext.Users.ToList());
        }

        [HttpPost]
        public IActionResult AddUsers(AddUserDto adduserdto)
        {
            User user = new()
            {
                UserName = adduserdto.UserName,
                Password = adduserdto.Password,
                Email = adduserdto.Email,
                PhoneNumber = adduserdto.PhoneNumber,
                Role = adduserdto.Role
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            if (user.Role.Equals("TheatreOwner"))
            {
                TheatreOwner theatreowner = new()
                {
                    TheatreOwnerId = user.UserId,
                    TheatreOwnerName = user.UserName
                };
                dbContext.TheatreOwners.Add(theatreowner);
                dbContext.SaveChanges();
                return Ok(theatreowner);
            }
            else if (user.Role.Equals("Customer"))
            {
                Customer customer = new()
                {
                    CustomerId = user.UserId,
                    CustomerName = user.UserName
                };
                dbContext.Customers.Add(customer);
                dbContext.SaveChanges();
                return Ok(customer);
            }
            else if (user.Role.Equals("Admin"))
            {
                Admin admin = new()
                {
                    AdminId = user.UserId,
                    AdminName = user.UserName
                };
                dbContext.Admins.Add(admin);
                dbContext.SaveChanges();
                return Ok(admin);
            }
            return Ok("User added successfully");
        }

        [HttpDelete]
        [Route("{userid:guid}")]
        public IActionResult DeleteUser(Guid userid)
        {
            var user = dbContext.Users.Find(userid);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            dbContext.Users.Remove(user);

            if (user.Role.Equals("TheatreOwner"))
            {
                var theatreOwner = dbContext.TheatreOwners.Find(userid);
                if (theatreOwner != null)
                {
                    var theatres = dbContext.Theatres.Where(t => t.TheatreOwnerId == theatreOwner.TheatreOwnerId).ToList();
                    foreach (var theatre in theatres)
                    {
                        var screens = dbContext.Screens.Where(s => s.TheatreId == theatre.TheatreId).ToList();
                        foreach (var screen in screens)
                        {
                            var shows = dbContext.Shows.Where(sh => sh.ScreenId == screen.ScreenId).ToList();
                            foreach (var show in shows)
                            {
                                var tickets = dbContext.Tickets.Where(t => t.ShowId == show.ShowId).ToList();
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
                var customer = dbContext.Customers.Find(userid);
                if (customer != null)
                {
                    var tickets = dbContext.Tickets.Where(t => t.CustomerId == customer.CustomerId).ToList();
                    foreach (var ticket in tickets)
                    {
                        dbContext.Tickets.Remove(ticket);
                    }
                    dbContext.Customers.Remove(customer);
                }
            }
            else if (user.Role.Equals("Admin"))
            {
                var admin = dbContext.Admins.Find(userid);
                if (admin != null)
                {
                    var movies = dbContext.Movies.Where(m => m.AdminId == admin.AdminId).ToList();
                    foreach (var movie in movies)
                    {
                        dbContext.Movies.Remove(movie);
                    }
                    dbContext.Admins.Remove(admin);
                }
            }

            dbContext.SaveChanges();
            return Ok("User deleted successfully");
        }
    }
}
