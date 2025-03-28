using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookMyShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly BookMyShowDbContext dbContext;

        public TicketsController(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("bookTicket")]
        public IActionResult BookTicket(BookTicketDto bookTicketDto)
        {
            Show? show = dbContext.Shows.Find(bookTicketDto.ShowId);
            if (show == null)
            {
                return BadRequest("Show not found");
            }

            Customer? customer = dbContext.Customers.Find(bookTicketDto.CustomerId);
            if (customer == null)
            {
                return BadRequest("Customer not found");
            }

            // Check if the user is a customer
            User? user = dbContext.Users.Find(customer.CustomerId);
            if (user == null || user.Role != "Customer")
            {
                return BadRequest("Only customers are allowed to book tickets");
            }

            // Check if the requested seats are available
            foreach (int seat in bookTicketDto.SeatNo)
            {
                if (!show.AvailableSeats.Contains(seat))
                {
                    return BadRequest($"Seat {seat} is not available");
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

            dbContext.Tickets.Add(ticket);
            dbContext.SaveChanges();

            // Save the updated available seats list
            dbContext.Entry(show).State = EntityState.Modified;
            dbContext.SaveChanges();

            return Ok(JsonConvert.SerializeObject(ticket, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        [HttpGet]
        [Route("availableSeats/{showId:guid}")]
        public IActionResult GetAvailableSeats(Guid showId)
        {
            Show? show = dbContext.Shows.Find(showId);
            if (show == null)
            {
                return NotFound("Show not found");
            }

            return Ok(new
            {
                TotalSeatsAvailable = show.AvailableSeats.Count,
                show.AvailableSeats
            });
        }

        [HttpDelete]
        [Route("cancelTicket/{ticketId:guid}")]
        public IActionResult CancelTicket(Guid ticketId)
        {
            Ticket? ticket = dbContext.Tickets.Find(ticketId);
            if (ticket == null)
            {
                return NotFound("Ticket not found");
            }

            Show? show = dbContext.Shows.Find(ticket.ShowId);
            if (show == null)
            {
                return NotFound("Show not found");
            }

            // Add the seats back to the available seats list
            foreach (int seat in ticket.SeatNo)
            {
                show.AvailableSeats.Add(seat);
            }

            dbContext.Tickets.Remove(ticket);
            dbContext.Entry(show).State = EntityState.Modified;
            dbContext.SaveChanges();

            return Ok("Ticket cancelled and seats restored");
        }

        [HttpGet]
        [Route("ticketDetails/{ticketId:guid}")]
        public IActionResult GetTicketDetails(Guid ticketId)
        {
            Ticket? ticket = dbContext.Tickets
                .Include(t => t.Customer)
                .Include(t => t.Show)
                    .ThenInclude(s => s.Movie)
                .Include(t => t.Show)
                    .ThenInclude(s => s.Screen)
                        .ThenInclude(sc => sc.Theatre)
                .FirstOrDefault(t => t.TicketId == ticketId);

            if (ticket == null)
            {
                return NotFound("Ticket not found");
            }

            var ticketDetails = new
            {
                ticket.TicketId,
                ticket.Customer.CustomerName,
                ticket.Show.Movie.Title,
                ShowDate = ticket.Show.ShowDate.ToString("dd/MM/yyyy"),
                ShowTime = DateTime.Today.Add(ticket.Show.ShowTime).ToString("hh:mm tt"), // Convert TimeSpan to DateTime
                ticket.Show.Screen.Theatre.TheatreName,
                ticket.Show.Screen.ScreenNumber,
                ticket.SeatNo,
                ticket.TicketPrice
            };

            return Ok(ticketDetails);
        }

        [HttpGet]
        [Route("bookedTickets")]
        public IActionResult GetBookedTickets()
        {
            var bookedTickets = dbContext.Tickets
                .Include(t => t.Customer)
                .Include(t => t.Show)
                .Select(t => new
                {
                    t.ShowId,
                    MovieName = t.Show.Movie.Title,
                    ShowDate = t.Show.ShowDate.ToString("dd/MM/yyyy"),
                    ShowTime = t.Show.ShowTime.ToString(@"hh\:mm tt"),
                    t.CustomerId,
                    t.Customer.CustomerName
                })
                .ToList();

            return Ok(bookedTickets);
        }

        [HttpGet]
        [Route("customerTickets/{customerId:guid}")]
        public IActionResult GetTicketsByCustomer(Guid customerId)
        {
            var customerTickets = dbContext.Tickets
                .Include(t => t.Customer)
                .Include(t => t.Show)
                .Where(t => t.CustomerId == customerId)
                .Select(t => new
                {
                    t.TicketId,
                    t.ShowId,
                    MovieName = t.Show.Movie.Title,
                    ShowDate = t.Show.ShowDate.ToString("dd/MM/yyyy"),
                    ShowTime = t.Show.ShowTime.ToString(@"hh\:mm tt"),
                    t.CustomerId,
                    t.Customer.CustomerName
                })
                .ToList();

            return Ok(customerTickets);
        }
    }
}
