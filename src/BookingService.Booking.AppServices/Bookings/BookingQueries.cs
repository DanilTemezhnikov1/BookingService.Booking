
using BookingService.Booking.AppServices.Queries;

namespace BookingService.Booking.AppServices.Bookings
{
    internal class BookingQueries : IBookingsQueries
    {
        public async Task<BookingData[]> GetByFilter(GetBookingsByFilterQuery getBookingsByFilter)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetStatusById(long? id)
        {
            throw new NotImplementedException();
        }
    }
}
