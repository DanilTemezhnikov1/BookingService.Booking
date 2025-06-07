namespace BookingService.Booking.AppServices.Bookings
{
    public enum BookingStatus
    {
        AwaitsConfirmation,
        Confirmed,
        Cancelled
    }
    public class BookingData
    {
        public long Id;
        public BookingStatus Status;
        public long IdUser;
        public long IdBooking;
        public DateOnly StartBooking;
        public DateOnly EndBooking;
        public DateTimeOffset CreationBooking;
    }
}
