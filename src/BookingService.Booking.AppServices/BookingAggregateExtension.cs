using BookingService.Booking.AppServices.Bookings;
using BookingService.Booking.Domain.Bookings;
using BookingService.Catalog.Api.Contracts.BookingJobs.Commands;

namespace BookingService.Booking.AppServices
{
    public static class BookingAggregateExtension
    {
        public static BookingData ToBookingData(this BookingAggregate? aggregate)
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
        public static CreateBookingJobCommand ToCreateBookingJobCommand(this BookingAggregate aggregate)
        {
            return new CreateBookingJobCommand
            {
                RequestId = aggregate.CatalogRequestId.Value,
                ResourceId = aggregate.Id,
                StartDate = aggregate.StartBooking,
                EndDate = aggregate.EndBooking,
            };
        }
    }
}
