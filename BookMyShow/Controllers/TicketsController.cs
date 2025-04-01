using BookMyShow.Exceptions;
using BookMyShow.Models;
using BookMyShow.Models.TicketDTOs;
using BookMyShow.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Books a ticket for a show.
        /// </summary>
        /// <param name="bookTicketDto">The ticket booking details.</param>
        /// <returns>The booked ticket.</returns>
        /// <exception cref="NotFoundException">Thrown when the show or customer is not found.</exception>
        /// <exception cref="BadRequestException">Thrown when the user role is invalid or requested seats are not available.</exception>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        [Route("bookTicket")]
        public async Task<IActionResult> BookTicket(BookTicketDto bookTicketDto)
        {
            try
            {
                TicketDetailsDto? response = await _ticketRepository.BookTicketAsync(bookTicketDto);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the available seats for a specific show.
        /// </summary>
        /// <param name="showId">The ID of the show to retrieve available seats for.</param>
        /// <returns>The available seats for the specified show.</returns>
        /// <exception cref="NotFoundException">Thrown when the show is not found.</exception>
        [Authorize(Roles = "TheatreOwner,Customer")]
        [HttpGet]
        [Route("availableSeats/{showId:guid}")]
        public async Task<IActionResult> GetAvailableSeats(Guid showId)
        {
            try
            {
                AvailableSeatsDto? response = await _ticketRepository.GetAvailableSeatsAsync(showId);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Cancels a ticket and restores the seats.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to cancel.</param>
        /// <returns>A message indicating the result of the cancellation.</returns>
        /// <exception cref="NotFoundException">Thrown when the ticket or show is not found.</exception>
        [Authorize(Roles = "Customer")]
        [HttpDelete]
        [Route("cancelTicket/{ticketId:guid}")]
        public async Task<IActionResult> CancelTicket(Guid ticketId)
        {
            try
            {
                string? response = await _ticketRepository.CancelTicketAsync(ticketId);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the details of a specific ticket.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to retrieve details for.</param>
        /// <returns>The details of the specified ticket.</returns>
        [Authorize(Roles = "TheatreOwner,Customer")]
        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetTicketDetails(Guid ticketId)
        {
            try
            {
                var ticketDetails = await _ticketRepository.GetTicketDetailsAsync(ticketId);
                return Ok(ticketDetails);
            }
            catch (TicketNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all booked tickets.
        /// </summary>
        /// <returns>A list of booked tickets.</returns>
        [Authorize(Roles = "TheatreOwner,Customer")]
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

        /// <summary>
        /// Retrieves all tickets booked by a specific customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve tickets for.</param>
        /// <returns>A list of tickets booked by the specified customer.</returns>
        [Authorize(Roles = "TheatreOwner,Customer")]
        [HttpGet]
        [Route("customerTickets/{customerId:guid}")]
        public async Task<IActionResult> GetTicketsByCustomer(Guid customerId)
        {
            List<BookedTicketsDto> customerTickets = await _ticketRepository.GetTicketsByCustomerAsync(customerId);
            if (!customerTickets.Any())
            {
                return NotFound($"No tickets found.");
            }
            return Ok(customerTickets);
        }
    }
}
