﻿using BookingService.Booking.AppServices.Bookings;
using BookingService.Booking.AppServices.Queries;

namespace BookingService.Booking.AppServices.Contracts;

public interface IBookingsQueries
{
    Task<BookingData[]> GetByFilter(GetBookingsByFilterQuery getBookingsByFilter, CancellationToken cancellationToken);
    Task<string> GetStatusById(long id, CancellationToken cancellationToken);
}