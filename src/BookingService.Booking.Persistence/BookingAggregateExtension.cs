using BookingService.Booking.AppServices.Bookings;
using BookingService.Booking.Domain.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Booking.Persistence
{ 
    public static class BookingAggregateExtension
    {
        public static BookingData ToBookingData(this BookingAggregate aggregate)
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
