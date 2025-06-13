using BookingService.Booking.Domain.Contracts.Bookings;
using BookingService.Booking.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace BookingService.Booking.Domain.Bookings
{
    public class BookingAggregate
    {
        public Guid? CatalogRequestId { get; private set; }
        [Key]
        public long Id { get; private set; }
        public BookingStatus Status { get; private set; }
        public long IdUser { get; private set; }
        public long IdBooking { get; private set; }
        public DateOnly StartBooking { get; private set; }
        public DateOnly EndBooking { get; private set; }
        public DateTimeOffset CreationBooking { get; private set; }

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
        public void SetCatalogRequestId(Guid catalogRequestId)
        {
            if (catalogRequestId == default) throw new DomainException();
            if (CatalogRequestId == null) CatalogRequestId = catalogRequestId;
            else throw new DomainException();
        }
        public static BookingAggregate Initialize(long idUser, long idBooking, DateOnly startBooking, DateOnly endBooking, DateTimeOffset creationBooking)
        {
            if (idUser <= 0 || idBooking <= 0) throw new DomainException("Id должен быть больше нуля");
            if (startBooking.CompareTo(DateOnly.FromDateTime(creationBooking.DateTime)) <= 0) throw new DomainException("Начало бронирования должно быть после текущей даты");
            if (endBooking.CompareTo(startBooking) <= 0) throw new DomainException("Окончание бронирования должно быть после начала бронирования");
            return new BookingAggregate(idUser, idBooking, startBooking, endBooking, creationBooking);
        }
        public void Confirm()
        {
            if (Status == BookingStatus.Confirmed) throw new DomainException("Статус уже подтверждён");
            if (Status == BookingStatus.Cancelled) throw new DomainException("Статус уже отменён");
            if (Status == BookingStatus.AwaitsConfirmation) Status = BookingStatus.Confirmed;
        }
        public void Cancel()
        {
            if (Status == BookingStatus.Cancelled) throw new DomainException("Статус уже отменён");
            Status = BookingStatus.Cancelled;
        }
    }
}
