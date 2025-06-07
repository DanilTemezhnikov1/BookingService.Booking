using BookingService.Booking.Domain.Contracts;

namespace BookingService.Booking.Domain.Bookings
{
    public class BookingAggregate
    {
        public long Id { get; set; }
        public BookingStatus Status { get; set; }
        public long IdUser { get; set; }
        public long IdBooking { get; set; }
        public DateOnly StartBooking { get; set; }
        public DateOnly EndBooking { get; set; }
        public DateTimeOffset CreationBooking { get; set; }

        private BookingAggregate(long idUser, long idBooking, DateOnly startBooking, DateOnly endBooking) 
        {
            IdUser = idUser;
            IdBooking = idBooking;
            StartBooking = startBooking;
            EndBooking = endBooking;
        }
        public static BookingAggregate Initialize(long idUser, long idBooking, DateOnly startBooking, DateOnly endBooking)
        {
            return new BookingAggregate(idUser, idBooking, startBooking, endBooking);
        }
    }
}
