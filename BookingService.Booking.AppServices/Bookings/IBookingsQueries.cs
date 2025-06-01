using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingService.Booking.Api.Contracts.Bookings.Dtos;
using static BookingService.Booking.Api.Contracts.Bookings.Dtos.BookingData;
namespace BookingService.Booking.AppServices.Bookings
{
    internal interface IBookingsQueries
    {
        BookingData[] GetByFilter();
        BookingStatus GetStatusById();
    }
}
