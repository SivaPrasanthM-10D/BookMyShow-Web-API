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
        public async Task<IActionResult> BookTicket(BookTicketDto bookTicketDto)
        {
            Ticket? response = await _ticketRepository.BookTicketAsync(bookTicketDto);
            if (response == null)
            {
                return BadRequest("Booking failed.");
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("availableSeats/{showId:guid}")]
        public async Task<IActionResult> GetAvailableSeats(Guid showId)
        {
            AvailableSeatsDto? response = await _ticketRepository.GetAvailableSeatsAsync(showId);
            if (response == null)
            {
                return NotFound("Show not found");
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("cancelTicket/{ticketId:guid}")]
        public async Task<IActionResult> CancelTicket(Guid ticketId)
        {
            string? response = await _ticketRepository.CancelTicketAsync(ticketId);
            if (response == null)
            {
                return NotFound("Ticket or Show not found");
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("ticketDetails/{ticketId:guid}")]
        public async Task<IActionResult> GetTicketDetails(Guid ticketId)
        {
            TicketDetailsDto? response = await _ticketRepository.GetTicketDetailsAsync(ticketId);
            if (response == null)
            {
                return NotFound("Ticket not found");
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("bookedTickets")]
        public async Task<IActionResult> GetBookedTickets()
        {
            List<BookedTicketsDto> bookedTickets = await _ticketRepository.GetBookedTicketsAsync();
            if (!bookedTickets.Any())
            {
                return NotFound("No booked tickets found.");
            }

            return Ok(bookedTickets);
        }

        [HttpGet]
        [Route("customerTickets/{customerId:guid}")]
        public async Task<IActionResult> GetTicketsByCustomer(Guid customerId)
        {
            List<BookedTicketsDto> customerTickets = await _ticketRepository.GetTicketsByCustomerAsync(customerId);
            if (!customerTickets.Any())
            {
                return NotFound($"No tickets found for {customerId}");
            }
            return Ok(customerTickets);
        }
    }
}
