namespace BookingService.Booking.AppServices.Dates;

public class DefaultCurrentDateTimeProvider : ICurrentDateTimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now.ToLocalTime();
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}