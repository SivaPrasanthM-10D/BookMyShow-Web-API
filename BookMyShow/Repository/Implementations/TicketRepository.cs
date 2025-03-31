using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Models;
using BookMyShow.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Repository.Implementations
{
    public class TicketRepository : ITicketRepository
    {
        private readonly BookMyShowDbContext dbContext;

        public TicketRepository(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Ticket?> BookTicketAsync(BookTicketDto bookTicketDto)
        {
            try
            {
                Show? show = await dbContext.Shows.FindAsync(bookTicketDto.ShowId);
                if (show == null)
                {
                    return null;
                }

                Customer? customer = await dbContext.Customers.FindAsync(bookTicketDto.CustomerId);
                if (customer == null)
                {
                    return null;
                }

                // Check if the user is a customer
                User? user = await dbContext.Users.FindAsync(customer.CustomerId);
                if (user == null || user.Role != "Customer")
                {
                    return null;
                }

                // Check if the requested seats are available
                foreach (int seat in bookTicketDto.SeatNo)
                {
                    if (!show.AvailableSeats.Contains(seat))
                    {
                        return null;
                    }
                }

                // Remove the booked seats from the available seats list
                foreach (int seat in bookTicketDto.SeatNo)
                {
                    show.AvailableSeats.Remove(seat);
                }

                Ticket ticket = new()
                {
                    CustomerId = bookTicketDto.CustomerId,
                    ShowId = show.ShowId,
                    SeatNo = bookTicketDto.SeatNo,
                    TicketPrice = show.TicketPrice
                };

                await dbContext.Tickets.AddAsync(ticket);
                await dbContext.SaveChangesAsync();

                // Save the updated available seats list
                dbContext.Entry(show).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();

                return ticket;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return null;
            }
        }

        public async Task<AvailableSeatsDto?> GetAvailableSeatsAsync(Guid showId)
        {
            try
            {
                Show? show = await dbContext.Shows.FindAsync(showId);
                if (show == null)
                {
                    return null;
                }

                return new AvailableSeatsDto
                {
                    TotalSeatsAvailable = show.AvailableSeats.Count,
                    AvailableSeats = show.AvailableSeats
                };
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return null;
            }
        }

        public async Task<string?> CancelTicketAsync(Guid ticketId)
        {
            try
            {
                Ticket? ticket = await dbContext.Tickets.FindAsync(ticketId);
                if (ticket == null)
                {
                    return null;
                }

                Show? show = await dbContext.Shows.FindAsync(ticket.ShowId);
                if (show == null)
                {
                    return null;
                }

                // Add the seats back to the available seats list
                foreach (int seat in ticket.SeatNo)
                {
                    show.AvailableSeats.Add(seat);
                }

                dbContext.Tickets.Remove(ticket);
                dbContext.Entry(show).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();

                return "Ticket cancelled and seats restored";
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return null;
            }
        }

        public async Task<TicketDetailsDto?> GetTicketDetailsAsync(Guid ticketId)
        {
            try
            {
                Ticket? ticket = await dbContext.Tickets
                    .Include(t => t.Customer)
                    .Include(t => t.Show)
                        .ThenInclude(s => s.Movie)
                    .Include(t => t.Show)
                        .ThenInclude(s => s.Screen)
                            .ThenInclude(sc => sc.Theatre)
                    .FirstOrDefaultAsync(t => t.TicketId == ticketId);

                if (ticket == null)
                {
                    return null;
                }

                var ticketDetails = new TicketDetailsDto
                {
                    TicketId = ticket.TicketId,
                    CustomerName = ticket.Customer.CustomerName,
                    MovieTitle = ticket.Show.Movie.Title,
                    ShowDate = ticket.Show.ShowDate.ToString("dd/MM/yyyy"),
                    ShowTime = DateTime.Today.Add(ticket.Show.ShowTime).ToString("hh:mm tt"),
                    TheatreName = ticket.Show.Screen.Theatre.TheatreName,
                    ScreenNumber = ticket.Show.Screen.ScreenNumber,
                    SeatNo = ticket.SeatNo,
                    TicketPrice = ticket.TicketPrice
                };

                return ticketDetails;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return null;
            }
        }

        public async Task<List<BookedTicketsDto>> GetBookedTicketsAsync()
        {
            try
            {
                List<BookedTicketsDto> bookedTickets = await dbContext.Tickets
                    .Include(t => t.Customer)
                    .Include(t => t.Show)
                    .Select(t => new BookedTicketsDto
                    {
                        TicketId = t.TicketId,
                        ShowId = t.ShowId,
                        MovieName = t.Show.Movie.Title,
                        ShowDate = t.Show.ShowDate.ToString("dd/MM/yyyy"),
                        ShowTime = DateTime.Today.Add(t.Show.ShowTime).ToString("hh:mm tt"),
                        CustomerId = t.CustomerId,
                        CustomerName = t.Customer.CustomerName
                    })
                    .ToListAsync();

                return bookedTickets;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return new List<BookedTicketsDto>();
            }
        }

        public async Task<List<BookedTicketsDto>> GetTicketsByCustomerAsync(Guid customerId)
        {
            try
            {
                List<BookedTicketsDto> customerTickets = await dbContext.Tickets
                    .Include(t => t.Customer)
                    .Include(t => t.Show)
                    .Where(t => t.CustomerId == customerId)
                    .Select(t => new BookedTicketsDto
                    {
                        TicketId = t.TicketId,
                        ShowId = t.ShowId,
                        MovieName = t.Show.Movie.Title,
                        ShowDate = t.Show.ShowDate.ToString("dd/MM/yyyy"),
                        ShowTime = t.Show.ShowTime.ToString(@"hh\:mm tt"),
                        CustomerId = t.CustomerId,
                        CustomerName = t.Customer.CustomerName
                    })
                    .ToListAsync();

                return customerTickets;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return new List<BookedTicketsDto>();
            }
        }
    }
}
