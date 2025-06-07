using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Booking.Domain.Contracts.Bookings
{
    public enum BookingStatus
    {
        AwaitsConfirmation,
        Confirmed,
        Cancelled
    }
}
