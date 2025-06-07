using BookingService.Booking.Domain.Bookings;
using BookingService.Booking.AppServices.Dates;
using BookingService.Booking.Domain.Contracts.Bookings;

namespace BookingService.Booking.Domain.UnitTests
{

    public class BookingAggregateTests
    {
        [Fact]
        public void Ctor_valid_all_arguments_creates_with_correct_ids_and_dates()
        {
            DefaultCurrentDateTimeProvider _currentDateTimeProvider = new DefaultCurrentDateTimeProvider();
            long idUser = 5;
            long idBooking = 5;
            DateOnly startBooking = new DateOnly(2026,3,5);
            DateOnly endBooking = new DateOnly(2026,4,5);
            DateTimeOffset creationBooking = _currentDateTimeProvider.Now;


           var bookingAggregate = BookingAggregate.Initialize(idUser, idBooking, startBooking, endBooking, creationBooking);


            Assert.Equal(idUser, bookingAggregate.IdUser);
            Assert.Equal(idBooking, bookingAggregate.IdBooking);
            Assert.Equal(startBooking, bookingAggregate.StartBooking);
            Assert.Equal(endBooking, bookingAggregate.EndBooking);
            Assert.Equal(creationBooking, bookingAggregate.CreationBooking);
        }
        [Fact]
        public void Ctor_id_user_less_than_or_equal_to_zero_throws_AE()
        {
            long idUser = 0;
            long idBooking = 14;


            Assert.Throws<ArgumentException>(() => BookingAggregate.Initialize(idUser, idBooking, new DateOnly(), new DateOnly(), new DateTimeOffset()));
        }
        [Fact]
        public void Ctor_id_booking_less_than_or_equal_to_zero_throws_AE()
        {
            long idUser = 7;
            long idBooking = -92;


            Assert.Throws<ArgumentException>(() => BookingAggregate.Initialize(idUser, idBooking, new DateOnly(), new DateOnly(), new DateTimeOffset()));
        }
        [Fact]
        public void Ctor_start_booking_less_than_or_equal_to_creation_booking_throws_AE()
        {
            DefaultCurrentDateTimeProvider _currentDateTimeProvider = new DefaultCurrentDateTimeProvider();
            DateOnly startBooking = new DateOnly(2004,6,30);
            DateTimeOffset creationBooking = _currentDateTimeProvider.Now;


            Assert.Throws<ArgumentException>(() => BookingAggregate.Initialize(long.MaxValue, long.MaxValue, startBooking, new DateOnly(), creationBooking));
        }
        [Fact]
        public void Ctor_end_booking_less_than_or_equal_to_start_booking_throws_AE()
        {
            DateOnly startBooking = new DateOnly(2025,12,12);
            DateOnly endBooking = new DateOnly(2004, 6, 30);


            Assert.Throws<ArgumentException>(() => BookingAggregate.Initialize(long.MaxValue, long.MaxValue, startBooking, endBooking, new DateTimeOffset()));
        }
        [Fact]
        public void Confirm_status_awaits_confirmation()
        {
            BookingAggregate bookingAggregate = new BookingAggregate();
            bookingAggregate.Status = BookingStatus.AwaitsConfirmation;


            bookingAggregate.Confirm();


            Assert.Equal(BookingStatus.Confirmed, bookingAggregate.Status);
        }
        [Fact]
        public void Confirm_status_confirmed_throws_IOE()
        {
            BookingAggregate bookingAggregate = new BookingAggregate();
            bookingAggregate.Status = BookingStatus.Confirmed;


            Assert.Throws<InvalidOperationException>(() => bookingAggregate.Confirm());
        }
        
        [Fact]
        public void Cancel_status_confirmed()
        {
            BookingAggregate bookingAggregate = new BookingAggregate();
            bookingAggregate.Status = BookingStatus.Confirmed;


            bookingAggregate.Cancel();


            Assert.Equal(BookingStatus.Cancelled, bookingAggregate.Status);
        }
        [Fact]
        public void Cancel_status_cancelled_throws_IOE()
        {
            BookingAggregate bookingAggregate = new BookingAggregate();
            bookingAggregate.Status = BookingStatus.Cancelled;


            Assert.Throws<InvalidOperationException>(() => bookingAggregate.Cancel());
        }
    }
}
