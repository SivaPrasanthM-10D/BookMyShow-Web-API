using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Exceptions;
using BookMyShow.Models;
using BookMyShow.Models.TicketDTOs;
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

        /// <summary>
        /// Books a ticket for a show.
        /// </summary>
        public async Task<TicketDetailsDto?> BookTicketAsync(BookTicketDto bookTicketDto)
        {
            Show? show = await dbContext.Shows.FindAsync(bookTicketDto.ShowId);
            if (show == null)
            {
                throw new ShowNotFoundException("Show not found.");
            }

            Customer? customer = await dbContext.Customers
                .Include(c => c.Tickets) // Ensure tickets are loaded
                .FirstOrDefaultAsync(c => c.CustomerId == bookTicketDto.CustomerId);
            if (customer == null)
            {
                throw new CustomerNotFoundException("Customer not found.");
            }

            // Check if the requested seats are available
            foreach (int seat in bookTicketDto.SeatNo)
            {
                if (!show.AvailableSeats.Contains(seat))
                {
                    throw new BadRequestException("Requested seats are not available.");
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
                TicketPrice = show.TicketPrice * bookTicketDto.SeatNo.Count
            };

            await dbContext.Tickets.AddAsync(ticket);
            customer.Tickets.Add(ticket); // Add ticket to customer's collection
            await dbContext.SaveChangesAsync();

            return await GetTicketDetailsAsync(ticket.TicketId);
        }

        /// <summary>
        /// Retrieves the available seats for a specific show.
        /// </summary>
        public async Task<AvailableSeatsDto?> GetAvailableSeatsAsync(Guid showId)
        {
            Show? show = await dbContext.Shows.FindAsync(showId);
            if (show == null)
            {
                throw new ShowNotFoundException("Show not found.");
            }

            return new AvailableSeatsDto
            {
                TotalSeatsAvailable = show.AvailableSeats.Count,
                AvailableSeats = show.AvailableSeats
            };
        }

        /// <summary>
        /// Cancels a ticket and restores the seats.
        /// </summary>
        public async Task<string?> CancelTicketAsync(Guid ticketId)
        {
            Ticket? ticket = await dbContext.Tickets
                .Include(t => t.Customer) // Ensure customer is loaded
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);
            if (ticket == null)
            {
                throw new TicketNotFoundException("Ticket not found.");
            }

            Show? show = await dbContext.Shows.FindAsync(ticket.ShowId);
            if (show == null)
            {
                throw new ShowNotFoundException("Show not found.");
            }

            // Add the seats back to the available seats list
            foreach (int seat in ticket.SeatNo)
            {
                show.AvailableSeats.Add(seat);
            }

            dbContext.Tickets.Remove(ticket);
            ticket.Customer.Tickets.Remove(ticket); // Remove ticket from customer's collection
            await dbContext.SaveChangesAsync();

            return "Ticket cancelled and seats restored";
        }

        /// <summary>
        /// Retrieves the details of a specific ticket.
        /// </summary>
        public async Task<TicketDetailsDto?> GetTicketDetailsAsync(Guid ticketId)
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
                throw new TicketNotFoundException("Ticket not found.");
            }

            return new TicketDetailsDto
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
        }

        /// <summary>
        /// Retrieves all booked tickets.
        /// </summary>
        public async Task<List<BookedTicketsDto>> GetBookedTicketsAsync()
        {
            return await dbContext.Tickets
                .Include(t => t.Customer)
                .Include(t => t.Show)
                .Select(t => new BookedTicketsDto
                {
                    TicketId = t.TicketId,
                    ShowId = t.ShowId,
                    MovieName = t.Show.Movie.Title,
                    TheatreName = t.Show.Screen.Theatre.TheatreName,
                    ShowDate = t.Show.ShowDate.ToString("dd/MM/yyyy"),
                    ShowTime = DateTime.Today.Add(t.Show.ShowTime).ToString("hh:mm tt"),
                    CustomerId = t.CustomerId,
                    CustomerName = t.Customer.CustomerName
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all tickets booked by a specific customer.
        /// </summary>
        public async Task<List<BookedTicketsDto>> GetTicketsByCustomerAsync(Guid customerId)
        {
            return await dbContext.Tickets
                .Include(t => t.Customer)
                .Include(t => t.Show)
                .Where(t => t.CustomerId == customerId)
                .Select(t => new BookedTicketsDto
                {
                    TicketId = t.TicketId,
                    ShowId = t.ShowId,
                    MovieName = t.Show.Movie.Title,
                    TheatreName = t.Show.Screen.Theatre.TheatreName,
                    ShowDate = t.Show.ShowDate.ToString("dd/MM/yyyy"),
                    ShowTime = DateTime.Today.Add(t.Show.ShowTime).ToString("hh:mm tt"),
                    CustomerId = t.CustomerId,
                    CustomerName = t.Customer.CustomerName
                })
                .ToListAsync();
        }
    }
}
