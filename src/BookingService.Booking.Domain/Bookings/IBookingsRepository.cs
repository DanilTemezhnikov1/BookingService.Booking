namespace BookingService.Booking.Domain.Bookings;

public interface IBookingsRepository
{
    public Task Create(BookingAggregate aggregate, CancellationToken cancellationToken);
    public ValueTask<BookingAggregate?> GetById(long id, CancellationToken token = default);
    public Task Update(BookingAggregate aggregate, CancellationToken cancellationToken);
}