using BookMyShow.Data.Entities;
using BookMyShow.Models;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        public readonly ITicketRepository _ticketRepository;

        public TicketsController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        [HttpPost]
        [Route("bookTicket")]
        public IActionResult BookTicket(BookTicketDto bookTicketDto)
        {
            Ticket? response = _ticketRepository.BookTicket(bookTicketDto);
            if (response == null)
            {
                return BadRequest("Booking failed.");
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("availableSeats/{showId:guid}")]
        public IActionResult GetAvailableSeats(Guid showId)
        {
            AvailableSeatsDto? response = _ticketRepository.GetAvailableSeats(showId);
            if (response == null)
            {
                return NotFound("Show not found");
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("cancelTicket/{ticketId:guid}")]
        public IActionResult CancelTicket(Guid ticketId)
        {
            string? response = _ticketRepository.CancelTicket(ticketId);
            if (response == null)
            {
                return NotFound("Ticket or Show not found");
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("ticketDetails/{ticketId:guid}")]
        public IActionResult GetTicketDetails(Guid ticketId)
        {
            TicketDetailsDto? response = _ticketRepository.GetTicketDetails(ticketId);

            if (response == null)
            {
                return NotFound("Ticket not found");
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("bookedTickets")]
        public IActionResult GetBookedTickets()
        {
            List<BookedTicketsDto> bookedTickets = _ticketRepository.GetBookedTickets();
            if (!bookedTickets.Any())
            {
                return NotFound("No booked tickets found.");
            }

            return Ok(bookedTickets);
        }

        [HttpGet]
        [Route("customerTickets/{customerId:guid}")]
        public IActionResult GetTicketsByCustomer(Guid customerId)
        {
            List<BookedTicketsDto> customerTickets = _ticketRepository.GetTicketsByCustomer(customerId);
            if (!customerTickets.Any())
            {
                return NotFound($"No tickets found for {customerId}");
            }
            return Ok(customerTickets);
        }
    }
}
