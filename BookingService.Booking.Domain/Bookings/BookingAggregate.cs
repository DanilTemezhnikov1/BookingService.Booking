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
        public DateTimeOffset CreationBooking {  get; set; }
        
        private BookingAggregate(long idUser, long idBooking, DateOnly startBooking, DateOnly endBooking)
        {
            Status = BookingStatus.AwaitsConfirmation;
            IdUser = idUser;
            IdBooking = idBooking;
            StartBooking = startBooking;
            EndBooking = endBooking;
        }
        public static BookingAggregate Initialize(long idUser, long idBooking, DateOnly startBooking, DateOnly endBooking)
        {
            if (idUser <= 0 || idBooking <=0) throw new ArgumentException("Id должен быть больше нуля");
            if (endBooking.CompareTo(startBooking) <= 0) throw new ArgumentException("Окончание бронирования должно быть после начала бронирования");
            return new BookingAggregate(idUser, idBooking, startBooking, endBooking);
        }
        public void Confirm ()
        {
            if (Status == BookingStatus.Confirmed) throw new InvalidOperationException("Статус уже подтверждён");
            if (Status == BookingStatus.Cancelled) throw new InvalidOperationException("Статус уже отменён");
            if (Status == BookingStatus.AwaitsConfirmation) Status = BookingStatus.Confirmed;
        }
        public void Cancel()
        {
            if (Status == BookingStatus.Cancelled) throw new InvalidOperationException("Статус уже отменён");
            Status = BookingStatus.Cancelled;
        }
    }
}
