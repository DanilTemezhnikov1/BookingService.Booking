using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Booking.Api.Contracts.Bookings.Dtos
{
    public class BookingData
    {
        public enum BookingStatus
        {
            AwaitsConfirmation,
            Confirmed,
            Cancelled
        }

        public long Id;
        public BookingStatus Status;
        public long IdUser;
        public long IdBooking;
        public DateOnly StartBooking;
        public DateOnly EndBooking;
        public DateTimeOffset CreationBooking;
    }
}
