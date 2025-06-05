namespace BookingService.Booking.AppServices.Queries
{
    public class CreateBookingQuery
    {
        public long IdUser { get; set; }
        public long IdBooking { get; set; }
        public DateOnly StartBooking { get; set; }
        public DateOnly EndBooking { get; set; }
    }

}
