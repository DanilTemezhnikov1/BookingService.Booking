using BookingService.Booking.Domain.Contracts.Bookings;

namespace BookingService.Booking.AppServices.Bookings
{
    public class BookingData
    {
        public long? Id;
        public BookingStatus Status;
        public long IdUser;
        public long IdBooking;
        public DateOnly StartBooking;
        public DateOnly EndBooking;
        public DateTimeOffset CreationBooking;
    }
}
