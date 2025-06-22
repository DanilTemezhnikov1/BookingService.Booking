using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Booking.Domain.Bookings
{
    public interface IBookingsBackgroundQueries
    {
        public IReadOnlyCollection<BookingAggregate> GetConfirmationAwaitingBookings(int countBookings = 10);
    }
}
