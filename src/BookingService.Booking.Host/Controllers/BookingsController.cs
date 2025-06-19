using BookingService.Booking.Api.Contracts.Bookings;
using BookingService.Booking.Api.Contracts.Bookings.Requests;
using BookingService.Booking.AppServices.Bookings;
using BookingService.Booking.AppServices;
using BookingService.Booking.Host.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Booking.Host.Controllers;

[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingsQueries _bookingsQueries;
    private readonly IBookingsService _bookingsService;

    public BookingsController(IBookingsService bookingsService, IBookingsQueries bookingsQueries, CancellationToken cancellationToken)
    {
        _bookingsService = bookingsService;
        _bookingsQueries = bookingsQueries;
    }

    [HttpPost]
    [Route(WebRoutes.Create)]
    public async Task<long> CreateBooking([FromBody] CreateBookingRequest createBookingRequest, CancellationToken cancellationToken)
    {
        return await _bookingsService.Create(createBookingRequest.ToQuery(), cancellationToken);
    }

    [HttpGet]
    [Route(WebRoutes.GetByFilter)]
    public async Task<BookingData[]> GetBookingsByFilter([FromQuery] GetBookingsByFilterRequest getBookingsByFilter, CancellationToken cancellationToken)
    {
        return await _bookingsQueries.GetByFilter(getBookingsByFilter.ToQuery(), cancellationToken);
    }

    [HttpPost]
    [Route(WebRoutes.Cancel)]
    public async Task CancelBooking([FromBody] long id, CancellationToken cancellationToken)
    {
        await _bookingsService.Cancel(id, cancellationToken);
    }

    [HttpGet]
    [Route(WebRoutes.GetById)]
    public async Task<BookingData> GetBookingById([FromRoute] long id, CancellationToken cancellationToken)
    {
        return await _bookingsService.GetById(id, cancellationToken);
    }

    [HttpGet]
    [Route(WebRoutes.GetStatusById)]
    public async Task<string> GetBookingStatusById([FromRoute] long id, CancellationToken cancellationToken)
    {
        return await _bookingsQueries.GetStatusById(id, cancellationToken);
    }
}