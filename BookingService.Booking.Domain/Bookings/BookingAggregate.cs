using BookingService.Booking.Domain.Contracts.Bookings;

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
        
        public BookingAggregate() { }
        private BookingAggregate(long idUser, long idBooking, DateOnly startBooking, DateOnly endBooking, DateTimeOffset creationBooking)
        {
            Status = BookingStatus.AwaitsConfirmation;
            IdUser = idUser;
            IdBooking = idBooking;
            StartBooking = startBooking;
            EndBooking = endBooking;
            CreationBooking = creationBooking;
        }
        public static BookingAggregate Initialize(long idUser, long idBooking, DateOnly startBooking, DateOnly endBooking, DateTimeOffset creationBooking)
        {
            if (idUser <= 0 || idBooking <=0) throw new ArgumentException("Id должен быть больше нуля");
            if (startBooking.CompareTo(DateOnly.FromDateTime(creationBooking.DateTime)) <= 0) throw new ArgumentException("Начало бронирования должно быть после текущей даты");
            if (endBooking.CompareTo(startBooking) <= 0) throw new ArgumentException("Окончание бронирования должно быть после начала бронирования");
            return new BookingAggregate(idUser, idBooking, startBooking, endBooking, creationBooking);
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
