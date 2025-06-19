using BookingService.Booking.Domain.Contracts.Bookings;

namespace BookingService.Booking.Api.Contracts.Bookings.Requests;

public record GetBookingsByFilterRequest(
    long? Id,
    BookingStatus? Status,
    long? IdUser,
    long? IdBooking,
    DateOnly? StartBooking,
    DateOnly? EndBooking,
    DateTimeOffset? CreationBooking);