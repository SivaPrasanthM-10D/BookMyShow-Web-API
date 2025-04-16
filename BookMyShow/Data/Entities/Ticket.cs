using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Data.Entities
{
    public class Ticket
    {
        [Key]
        public Guid TicketId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        // Updated navigation property to reflect the relationship with Customer
        public Customer Customer { get; set; }

        [Required]
        public List<int> SeatNo { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal TicketPrice { get; set; }

        [Required]
        public Guid ShowId { get; set; }

        // Updated navigation property to reflect the relationship with Show
        public Show Show { get; set; }
    }
}
