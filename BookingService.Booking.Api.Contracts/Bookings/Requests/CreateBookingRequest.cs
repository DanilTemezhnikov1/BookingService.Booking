namespace BookingService.Booking.Api.Contracts.Bookings.Requests
{
    public record CreateBookingRequest(long IdUser,
                                       long IdBooking,
                                       DateOnly StartBooking,
                                       DateOnly EndBooking);
}