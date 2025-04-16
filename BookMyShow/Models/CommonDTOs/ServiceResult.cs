
namespace BookMyShow.Models.CommonDTOs
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; internal set; }
    }
}
