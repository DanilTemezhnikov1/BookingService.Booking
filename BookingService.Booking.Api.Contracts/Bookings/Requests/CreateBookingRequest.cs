using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookingService.Booking.Api.Contracts.Bookings.Dtos.BookingDataResponse;

namespace BookingService.Booking.Api.Contracts.Bookings.Requests
{
    public record CreateBookingRequest(long IdUser,
                                        long IdBooking, 
                                        DateOnly StartBooking,
                                        DateOnly EndBooking);
}                    