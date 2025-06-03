using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BookingService.Booking.AppServices.Bookings
{
    public interface IBookingsService
    {
        long Create(long? IdUser,long? IdBooking, DateOnly? StartBooking, DateOnly? EndBooking);
        BookingData GetById(long? id);
        void Cancel(long? id);
    }
}
