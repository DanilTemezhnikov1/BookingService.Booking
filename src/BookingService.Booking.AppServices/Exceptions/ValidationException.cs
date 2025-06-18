namespace BookingService.Booking.AppServices.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
    {
    }

    public ValidationException(string str) : base(str)
    {
    }
}