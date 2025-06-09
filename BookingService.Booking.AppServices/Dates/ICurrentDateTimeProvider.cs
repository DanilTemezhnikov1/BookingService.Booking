namespace BookingService.Booking.AppServices.Dates
{
    public interface ICurrentDateTimeProvider
    {
        public DateTimeOffset Now { get; }
        public DateTimeOffset UtcNow { get; }
    }
}

