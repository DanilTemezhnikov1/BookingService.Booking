
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Booking.AppServices.Bookings
{
    internal class BookingQueries : IBookingsQueries
    {
        public BookingData[] GetByFilter(BookingData.BookingStatus? status, long? idUser, long? idBooking, DateOnly? startBooking, DateOnly? endBooking)
        {
            throw new NotImplementedException();
        }

        public BookingData.BookingStatus GetStatusById(long? id)
        {
            throw new NotImplementedException();
        }
    }
}
