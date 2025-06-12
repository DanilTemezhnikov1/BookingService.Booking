using BookingService.Booking.AppServices.Bookings;
using BookingService.Booking.AppServices.Contracts;
using BookingService.Booking.AppServices.Queries;
using BookingService.Booking.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Booking.Persistence
{
    internal class BookingQueries : IBookingsQueries
    {
        private readonly BookingsContext _context;

        public BookingQueries(BookingsContext context)
        {
            _context = context;
        }
        public async Task<BookingData[]> GetByFilter(GetBookingsByFilterQuery getBookingsByFilter)
        {
            var bookingsQuery = _context.Bookings.AsQueryable();

            if (getBookingsByFilter.Id.HasValue)
                bookingsQuery = bookingsQuery
                    .Where(x => x.Id == getBookingsByFilter.Id.Value);

            if (getBookingsByFilter.Status.HasValue)
                bookingsQuery = bookingsQuery
                    .Where(x => x.Status == getBookingsByFilter.Status.Value);

            if (getBookingsByFilter.IdUser.HasValue)
                bookingsQuery = bookingsQuery
                    .Where(x => x.IdUser == getBookingsByFilter.IdUser.Value);

            if (getBookingsByFilter.IdBooking.HasValue)
                bookingsQuery = bookingsQuery
                    .Where(x => x.IdBooking == getBookingsByFilter.IdBooking.Value);

            if (getBookingsByFilter.StartBooking.HasValue)
                bookingsQuery = bookingsQuery
                    .Where(x => x.StartBooking == getBookingsByFilter.StartBooking.Value);

            if (getBookingsByFilter.EndBooking.HasValue)
                bookingsQuery = bookingsQuery
                    .Where(x => x.EndBooking == getBookingsByFilter.EndBooking.Value);

            if (getBookingsByFilter.CreationBooking.HasValue)
                bookingsQuery = bookingsQuery
                    .Where(x => x.CreationBooking == getBookingsByFilter.CreationBooking.Value);

            return await bookingsQuery.Select(x => x.ToBookingData()).ToArrayAsync();

        }
        public async Task<string> GetStatusById(long id)
        {
            var booking = await _context.FindAsync<BookingAggregate>(id);
            return booking.Status.ToString();
        }
    }
}
