using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BookingService.Booking.AppServices.Bookings
{
    public interface IBookingsQueries
    {
        BookingData[] GetByFilter(BookingData.BookingStatus? status, long? idUser, long? idBooking, DateOnly? startBooking, DateOnly? endBooking);
        BookingData.BookingStatus GetStatusById(long? id);
    }
}
