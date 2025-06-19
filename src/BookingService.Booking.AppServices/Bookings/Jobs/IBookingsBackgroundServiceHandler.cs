namespace BookingService.Booking.AppServices.Bookings.Jobs
{
    public interface IBookingsBackgroundServiceHandler
    {
        public Task Handle(CancellationToken cancellationToken);
    }
}
