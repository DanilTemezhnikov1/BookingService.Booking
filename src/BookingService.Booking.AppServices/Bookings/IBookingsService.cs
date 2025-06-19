using BookingService.Booking.AppServices.Queries;

namespace BookingService.Booking.AppServices.Bookings;

public interface IBookingsService
{
    Task<long> Create(CreateBookingQuery createBooking, CancellationToken cancellationToken);
    Task<BookingData> GetById(long id, CancellationToken cancellationToken);
    Task Cancel(long id , CancellationToken cancellationToken);
}