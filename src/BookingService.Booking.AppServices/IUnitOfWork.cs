namespace BookingService.Booking.AppServices;

public interface IUnitOfWork
{
    public IBookingsRepository BookingsRepository { get; }
    Task CommitAsync(CancellationToken cancellationToken = default);
}