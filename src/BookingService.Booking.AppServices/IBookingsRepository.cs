using BookingService.Booking.Domain.Bookings;

namespace BookingService.Booking.AppServices;

public interface IBookingsRepository
{
    public Task Create(BookingAggregate aggregate, CancellationToken cancellationToken);
    public ValueTask<BookingAggregate?> GetById(long id, CancellationToken token = default);
    public Task Update(BookingAggregate aggregate, CancellationToken cancellationToken);
}