using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Data.Entities
{
    public class Admin
    {
        [Key]
        public Guid AdminId { get; set; }
        [Required]
        [StringLength(100)]
        public string AdminName { get; set; }
        public Guid MovieId { get;  set; }
        public ICollection<Movie> Movies { get;  set; }
    }
}
