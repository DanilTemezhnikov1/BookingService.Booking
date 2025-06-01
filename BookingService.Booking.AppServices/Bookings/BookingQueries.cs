using BookingService.Booking.Api.Contracts.Bookings.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Booking.AppServices.Bookings
{
    internal class BookingQueries : IBookingsQueries
    {
        public BookingData[] GetByFilter()
        {
            throw new NotImplementedException();
        }

        public BookingData.BookingStatus GetStatusById()
        {
            throw new NotImplementedException();
        }
    }
}
