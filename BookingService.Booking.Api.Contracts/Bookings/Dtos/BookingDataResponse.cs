using BookingService.Booking.Domain.Contracts;
namespace BookingService.Booking.Api.Contracts.Bookings.Dtos
{
   
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
