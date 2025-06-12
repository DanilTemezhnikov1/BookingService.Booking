using BookingService.Booking.AppServices.Bookings;
using BookingService.Booking.Domain.Bookings;

namespace BookingService.Booking.AppServices
{
    public static class BookingAggregateExtension
    {
        public static async Task<BookingData> ToBookingData(this BookingAggregate? aggregate)
        {
            return new BookingData
            {
                Id = aggregate.Id,
                Status = aggregate.Status,
                IdUser = aggregate.IdUser,
                IdBooking = aggregate.IdBooking,
                StartBooking = aggregate.StartBooking,
                EndBooking = aggregate.EndBooking,
                CreationBooking = aggregate.CreationBooking
            };
        }
    }
}
