using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Data.Entities
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        // Updated to ICollection to reflect a one-to-many relationship
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
