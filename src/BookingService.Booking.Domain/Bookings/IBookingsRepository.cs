namespace BookingService.Booking.Domain.Bookings
{
    public interface IBookingsRepository
    {
        public void Create(BookingAggregate aggregate);
        public ValueTask<BookingAggregate?> GetById(long id, CancellationToken token = default);
        public void Update(BookingAggregate aggregate);

    }
}
