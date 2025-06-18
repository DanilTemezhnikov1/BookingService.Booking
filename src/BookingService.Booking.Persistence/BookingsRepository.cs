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

    public void Create(BookingAggregate aggregate)
    {
        _dbset.Add(aggregate);
    }

    public async ValueTask<BookingAggregate?> GetById(long id, CancellationToken token = default)
    {
        return await _dbset.FindAsync(id, token);
    }

    public void Update(BookingAggregate aggregate)
    {
        _dbset.Attach(aggregate);
        _dbset.Entry(aggregate).State = EntityState.Modified;
    }
}