using BookingService.Booking.AppServices;
using BookingService.Booking.Domain.Bookings;

namespace BookingService.Booking.Domain;

public interface IUnitOfWork
{
    public IBookingsRepository BookingsRepository { get; }
    Task CommitAsync(CancellationToken cancellationToken = default);
}