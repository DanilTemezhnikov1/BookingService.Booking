using BookingService.Booking.AppServices;
using BookingService.Booking.Domain;

namespace BookingService.Booking.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly BookingsContext _dbContext;

    public UnitOfWork(BookingsContext dbContext, IBookingsRepository bookingsRepository)
    {
        _dbContext = dbContext;
        BookingsRepository = bookingsRepository;
    }

    public IBookingsRepository BookingsRepository { get; }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}