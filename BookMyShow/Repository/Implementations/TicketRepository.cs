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
        /// <param name="bookTicketDto">The ticket booking details.</param>
        /// <returns>The booked ticket.</returns>
        /// <exception cref="ShowNotFoundException">Thrown when the show is not found.</exception>
        /// <exception cref="CustomerNotFoundException">Thrown when the customer is not found.</exception>
        /// <exception cref="InvalidTicketDataException">Thrown when the user role is invalid or requested seats are not available.</exception>
        public async Task<TicketDetailsDto?> BookTicketAsync(BookTicketDto bookTicketDto)
        {
            Show? show = await dbContext.Shows.FindAsync(bookTicketDto.ShowId);
            if (show == null)
            {
                throw new ShowNotFoundException("Show not found.");
            }

            Customer? customer = await dbContext.Customers.FindAsync(bookTicketDto.CustomerId);
            if (customer == null)
            {
                throw new CustomerNotFoundException("Customer not found.");
            }

            // Check if the user is a customer
            User? user = await dbContext.Users.FindAsync(customer.CustomerId);
            if (user == null || user.Role != "Customer")
            {
                throw new BadRequestException("Invalid user role.");
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
            await dbContext.SaveChangesAsync();

            // Save the updated available seats list
            dbContext.Entry(show).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            var ticketDetails = await GetTicketDetailsAsync(ticket.TicketId);
            return ticketDetails;
        }

        /// <summary>
        /// Retrieves the available seats for a specific show.
        /// </summary>
        /// <param name="showId">The ID of the show to retrieve available seats for.</param>
        /// <returns>The available seats for the specified show.</returns>
        /// <exception cref="ShowNotFoundException">Thrown when the show is not found.</exception>
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
        /// <param name="ticketId">The ID of the ticket to cancel.</param>
        /// <returns>A message indicating the result of the cancellation.</returns>
        /// <exception cref="TicketNotFoundException">Thrown when the ticket is not found.</exception>
        /// <exception cref="ShowNotFoundException">Thrown when the show is not found.</exception>
        public async Task<string?> CancelTicketAsync(Guid ticketId)
        {
            Ticket? ticket = await dbContext.Tickets.FindAsync(ticketId);
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
            dbContext.Entry(show).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return "Ticket cancelled and seats restored";
        }

        /// <summary>
        /// Retrieves the details of a specific ticket.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to retrieve details for.</param>
        /// <returns>The details of the specified ticket.</returns>
        /// <exception cref="TicketNotFoundException">Thrown when the ticket is not found.</exception>
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

        /// <summary>
        /// Retrieves all booked tickets.
        /// </summary>
        /// <returns>A list of booked tickets.</returns>
        public async Task<List<BookedTicketsDto>> GetBookedTicketsAsync()
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

        /// <summary>
        /// Retrieves all tickets booked by a specific customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve tickets for.</param>
        /// <returns>A list of tickets booked by the specified customer.</returns>
        public async Task<List<BookedTicketsDto>> GetTicketsByCustomerAsync(Guid customerId)
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
                    ShowTime = DateTime.Today.Add(t.Show.ShowTime).ToString("hh:mm tt"),
                    CustomerId = t.CustomerId,
                    CustomerName = t.Customer.CustomerName
                })
                .ToListAsync();

            return customerTickets;
        }
    }
}
