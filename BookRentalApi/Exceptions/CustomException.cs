namespace BookRentalApi.Exceptions
{
    public class CustomException : Exception
    {
        public int StatusCode { get; }

        public CustomException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    // Specific Exception for Book Not Found
    public class BookNotFoundException : CustomException
    {
        public BookNotFoundException(int bookId)
            : base($"Book with ID {bookId} not found.", 404) { }
    }

    // Specific Exception for User Not Found
    public class UserNotFoundException : CustomException
    {
        public UserNotFoundException(int userId)
            : base($"User with ID {userId} not found.", 404) { }
    }

    // Specific Exception for Rental Failure
    public class RentalException : CustomException
    {
        public RentalException(string message)
            : base(message, 400) { }
    }

    // Other Custom Exceptions
    public class OverdueRentalException : CustomException
    {
        public OverdueRentalException(string message)
            : base(message, 500) { }
    }
}

