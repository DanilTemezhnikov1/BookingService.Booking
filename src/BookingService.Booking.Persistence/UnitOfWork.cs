using BookingService.Booking.Domain;
using BookingService.Booking.Domain.Bookings;

namespace BookingService.Booking.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookingsContext _dbContext;
        public IBookingsRepository BookingsRepository { get; }
        public UnitOfWork(BookingsContext dbContext, IBookingsRepository bookingsRepository)
        {
            _dbContext = dbContext;
            BookingsRepository = bookingsRepository;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
