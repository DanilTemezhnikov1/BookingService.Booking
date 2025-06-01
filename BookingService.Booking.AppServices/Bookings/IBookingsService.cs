using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingService.Booking.Api.Contracts.Bookings.Dtos;
using static BookingService.Booking.Api.Contracts.Bookings.Dtos.BookingData;
namespace BookingService.Booking.AppServices.Bookings
{
    internal interface IBookingsService
    {
        long Create(long Id,
                    BookingStatus Status,
                    long IdUser,
                    long IdBooking,
                    DateOnly StartBooking,
                    DateOnly EndBooking,
                    DateTimeOffset CreationBooking);
        BookingData GetById(long id);
        void Cancel(long id);
    }
}
