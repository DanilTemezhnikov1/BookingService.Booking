using BookingService.Booking.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Booking.Persistence
{
    public class BookingsRepository : IBookingsRepository
    {
        private DbSet<BookingAggregate> _dbset;

        public BookingsRepository(BookingsContext context)
        {
            _dbset = context.Bookings;
        }
        public void Create(BookingAggregate aggregate)
        {
            _dbset.Add(aggregate);
        }


        public ValueTask<BookingAggregate?> GetById(long id, CancellationToken token)
        {
            return _dbset.FindAsync(id, token);
        }

        public void Update(BookingAggregate aggregate)
        {
            _dbset.Attach(aggregate);
            _dbset.Entry(aggregate).State = EntityState.Modified;
        }
    }
}
