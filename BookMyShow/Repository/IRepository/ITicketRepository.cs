using BookMyShow.Models;
using BookMyShow.Models.TicketDTOs;

namespace BookMyShow.Repository.IRepository
{
    public interface ITicketRepository
    {
        Task<TicketDetailsDto?> BookTicketAsync(BookTicketDto bookTicketDto);
        Task<AvailableSeatsDto?> GetAvailableSeatsAsync(Guid showId);
        Task<string?> CancelTicketAsync(Guid ticketId);
        Task<TicketDetailsDto?> GetTicketDetailsAsync(Guid ticketId);
        Task<List<BookedTicketsDto>> GetBookedTicketsAsync();
        Task<List<BookedTicketsDto>> GetTicketsByCustomerAsync(Guid customerId);
    }
}
