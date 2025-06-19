using BookingService.Booking.Domain.Contracts.Bookings;

namespace BookingService.Booking.Api.Contracts.Bookings.Dtos;

public class BookingDataResponse
{
    public DateTimeOffset CreationBooking;
    public DateOnly EndBooking;
    public long Id;
    public long IdBooking;
    public long IdUser;
    public DateOnly StartBooking;
    public BookingStatus Status;
}