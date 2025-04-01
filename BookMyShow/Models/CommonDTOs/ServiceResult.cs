using BookMyShow.Data.Entities;

namespace BookMyShow.Models.CommonDTOs
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public User User { get; internal set; }
    }
}
