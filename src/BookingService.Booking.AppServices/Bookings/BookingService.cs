using BookingService.Booking.AppServices.Exceptions;
using BookingService.Booking.AppServices.Queries;
using BookingService.Booking.Domain.Exceptions;


namespace BookingService.Booking.AppServices.Bookings
{
    internal class BookingService : IBookingsService
    {
        public async Task<long> Create(CreateBookingQuery createBooking)
        {
            throw new ValidationException();
        }
        public async Task<BookingData> GetById(long? id)
        {
            throw new NotImplementedException();
        }
        public async Task Cancel(long? id)
        {
            throw new DomainException();
        }
    }
}
