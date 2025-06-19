using BookingService.Booking.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Booking.Persistence;

public class BookingsRepository : IBookingsRepository
{
    private readonly DbSet<BookingAggregate> _dbset;

    public BookingsRepository(BookingsContext context)
    {
        _dbset = context.Bookings;
    }

    public async Task Create(BookingAggregate aggregate, CancellationToken cancellationToken)
    {
         await _dbset.AddAsync(aggregate, cancellationToken);
    }

    public async ValueTask<BookingAggregate?> GetById(long id, CancellationToken token = default)
    {
        return await _dbset.FindAsync(id, token);
    }

    public async Task Update(BookingAggregate aggregate, CancellationToken cancellationToken)
    {
        _dbset.Attach(aggregate);
        _dbset.Entry(aggregate).State = EntityState.Modified;
    }
}