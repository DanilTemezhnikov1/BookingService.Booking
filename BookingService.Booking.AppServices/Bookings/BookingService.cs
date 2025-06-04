using BookingService.Booking.Domain.Exceptions;
using BookingService.Booking.AppServices.Exceptions;


namespace BookingService.Booking.AppServices.Bookings
{
    internal class BookingService : IBookingsService
    {
        public long Create(long? IdUser, long? IdBooking, DateOnly? StartBooking, DateOnly? EndBooking)
        {
            throw new ValidationException();
        }
        public BookingData GetById(long? id)
        {
            throw new NotImplementedException();
        }
        public void Cancel(long? id)
        {
            throw new DomainException();
        }
    }
}
