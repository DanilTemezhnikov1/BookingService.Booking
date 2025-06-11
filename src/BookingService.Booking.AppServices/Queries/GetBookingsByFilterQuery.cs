using BookingService.Booking.Domain.Contracts.Bookings;

namespace BookingService.Booking.AppServices.Queries
{
    public class GetBookingsByFilterQuery
    {
        public long? Id { get; set; }
        public BookingStatus? Status { get; set; }
        public long? IdUser { get; set; }
        public long? IdBooking { get; set; }
        public DateOnly? StartBooking { get; set; }
        public DateOnly? EndBooking { get; set; }
        public DateTimeOffset? CreationBooking { get; set; }
    }
}
