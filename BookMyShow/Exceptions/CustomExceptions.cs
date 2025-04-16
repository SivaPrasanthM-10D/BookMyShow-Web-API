namespace BookMyShow.Exceptions
{
    public class DuplicateRecordException : Exception
    {
        public DuplicateRecordException(string message) : base(message) { }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string? message) : base(message)
        {
        }
    }

    public class TheatreNotFoundException : Exception
    {
        public TheatreNotFoundException() : base() { }
        public TheatreNotFoundException(string message) : base(message) { }
        public TheatreNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ScreenNotFoundException : Exception
    {
        public ScreenNotFoundException() : base() { }
        public ScreenNotFoundException(string message) : base(message) { }
        public ScreenNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ShowNotFoundException : Exception
    {
        public ShowNotFoundException() : base() { }
        public ShowNotFoundException(string message) : base(message) { }
        public ShowNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ReviewNotFoundException : Exception
    {
        public ReviewNotFoundException() : base() { }
        public ReviewNotFoundException(string message) : base(message) { }
        public ReviewNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InvalidReviewDataException : Exception
    {
        public InvalidReviewDataException() : base() { }
        public InvalidReviewDataException(string message) : base(message) { }
        public InvalidReviewDataException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class MovieNotFoundException : Exception
    {
        public MovieNotFoundException() : base() { }
        public MovieNotFoundException(string message) : base(message) { }
        public MovieNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class TicketNotFoundException : Exception
    {
        public TicketNotFoundException() : base() { }
        public TicketNotFoundException(string message) : base(message) { }
        public TicketNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InvalidTicketDataException : Exception
    {
        public InvalidTicketDataException() : base() { }
        public InvalidTicketDataException(string message) : base(message) { }
        public InvalidTicketDataException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException() : base() { }
        public CustomerNotFoundException(string message) : base(message) { }
        public CustomerNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class TheatreOwnerNotFoundException : Exception
    {
        public TheatreOwnerNotFoundException() : base() { }
        public TheatreOwnerNotFoundException(string message) : base(message) { }
        public TheatreOwnerNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
