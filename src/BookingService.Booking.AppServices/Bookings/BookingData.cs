using BookingService.Booking.Domain.Contracts.Bookings;

namespace BookingService.Booking.AppServices.Bookings;

public class BookingData
{
    public DateTimeOffset CreationBooking;
    public DateOnly EndBooking;
    public long Id;
    public long IdBooking;
    public long IdUser;
    public DateOnly StartBooking;
    public BookingStatus Status;
}