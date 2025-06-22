using BookingService.Booking.Domain.Bookings;
using BookingService.Booking.Domain.Contracts.Bookings;

namespace BookingService.Booking.Persistence
{
    public class BookingsBackgroundQueries : IBookingsBackgroundQueries
    {
        private BookingsContext _bookingsContext;
        public BookingsBackgroundQueries(BookingsContext bookingsContext)
        {
            _bookingsContext = bookingsContext;
        }
        public IReadOnlyCollection<BookingAggregate> GetConfirmationAwaitingBookings(int countBookings = 10)
        {
            return _bookingsContext.Bookings
               .Where(x => x.Status == BookingStatus.AwaitsConfirmation)
               .OrderBy(x => x.Id)
               .Take(countBookings)
               .ToList()
               .AsReadOnly();
        }
    }
}
