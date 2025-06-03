using BookingService.Booking.Domain.Exceptions;
using BookingService.Booking.AppServices.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Booking.AppServices.Bookings
{
    internal class BookingService : IBookingsService
    {
        public long Create(long? IdUser, long? IdBooking, DateOnly? StartBooking, DateOnly? EndBooking)
        {
            throw new ValidationException();
        }
        public BookingData GetById(long? id)
        {
            throw new NotImplementedException();
        }
        public void Cancel(long? id)
        {
            throw new DomainException();
        }
    }
}
