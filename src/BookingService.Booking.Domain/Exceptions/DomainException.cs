namespace BookingService.Booking.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException()
    {
    }

    public DomainException(string str) : base(str)
    {
    }
}