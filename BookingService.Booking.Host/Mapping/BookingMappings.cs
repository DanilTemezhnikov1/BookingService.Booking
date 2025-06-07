using BookingService.Booking.Api.Contracts.Bookings.Requests;
using BookingService.Booking.Domain.Contracts;
using BookingService.Booking.AppServices.Queries;

namespace BookingService.Booking.Host.Mapping
{
    public static class BookingMappings
    {
        public static CreateBookingQuery ToQuery(this CreateBookingRequest request) =>
            new CreateBookingQuery
            {
                IdUser = request.IdUser,
                IdBooking = request.IdBooking,
                StartBooking = request.StartBooking,
                EndBooking = request.EndBooking,
            };
        public static GetBookingsByFilterQuery ToQuery(this GetBookingsByFilterRequest request) =>
            new GetBookingsByFilterQuery
            {
                Id = request.Id,
                IdUser = request.IdUser,
                IdBooking = request.IdBooking,
                Status = request.Status,
                CreationBooking = request.CreationBooking,
                StartBooking = request.StartBooking,
                EndBooking = request.EndBooking
            };
    }
}
