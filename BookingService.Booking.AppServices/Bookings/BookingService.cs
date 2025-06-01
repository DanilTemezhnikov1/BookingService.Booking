using BookingService.Booking.Api.Contracts.Bookings.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Booking.AppServices.Bookings
{
    internal class BookingService : IBookingsService
    {
        public long Create(long Id, BookingData.BookingStatus Status, long IdUser, long IdBooking, DateOnly StartBooking, DateOnly EndBooking, DateTimeOffset CreationBooking)
        {
            throw new NotImplementedException();
        }
        public BookingData GetById(long id)
        {
            throw new NotImplementedException();
        }
        public void Cancel(long id)
        {
            throw new NotImplementedException();
        }
    }
}
