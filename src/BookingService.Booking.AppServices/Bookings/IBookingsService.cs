using BookingService.Booking.AppServices.Queries;

namespace BookingService.Booking.AppServices.Bookings;

public interface IBookingsService
{
    Task<long> Create(CreateBookingQuery createBooking);
    Task<BookingData> GetById(long id);
    Task Cancel(long id);
}