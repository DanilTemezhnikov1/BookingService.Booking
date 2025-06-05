namespace BookingService.Booking.Api.Contracts.Bookings.Dtos
{
    public enum BookingStatus
    {
        AwaitsConfirmation,
        Confirmed,
        Cancelled
    }
    public class BookingDataResponse
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
