using BookMyShow.Data.Entities;
using BookMyShow.Models;

namespace BookMyShow.Repository.IRepository
{
    public interface ITicketRepository
    {
        Task<Ticket?> BookTicketAsync(BookTicketDto bookTicketDto);
        Task<AvailableSeatsDto?> GetAvailableSeatsAsync(Guid showId);
        Task<string?> CancelTicketAsync(Guid ticketId);
        Task<TicketDetailsDto?> GetTicketDetailsAsync(Guid ticketId);
        Task<List<BookedTicketsDto>> GetBookedTicketsAsync();
        Task<List<BookedTicketsDto>> GetTicketsByCustomerAsync(Guid customerId);
    }
}
