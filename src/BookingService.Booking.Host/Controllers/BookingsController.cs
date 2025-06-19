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
    private CancellationToken _cancellationToken;

    public BookingsController(IBookingsService bookingsService, IBookingsQueries bookingsQueries, CancellationToken cancellationToken)
    {
        _bookingsService = bookingsService;
        _bookingsQueries = bookingsQueries;
        _cancellationToken = cancellationToken;
    }

    [HttpPost]
    [Route(WebRoutes.Create)]
    public async Task<long> CreateBooking([FromBody] CreateBookingRequest createBookingRequest)
    {
        return await _bookingsService.Create(createBookingRequest.ToQuery(), _cancellationToken);
    }

    [HttpGet]
    [Route(WebRoutes.GetByFilter)]
    public async Task<BookingData[]> GetBookingsByFilter([FromQuery] GetBookingsByFilterRequest getBookingsByFilter)
    {
        return await _bookingsQueries.GetByFilter(getBookingsByFilter.ToQuery(), _cancellationToken);
    }

    [HttpPost]
    [Route(WebRoutes.Cancel)]
    public async Task CancelBooking([FromBody] long id)
    {
        await _bookingsService.Cancel(id, _cancellationToken);
    }

    [HttpGet]
    [Route(WebRoutes.GetById)]
    public async Task<BookingData> GetBookingById([FromRoute] long id)
    {
        return await _bookingsService.GetById(id, _cancellationToken);
    }

    [HttpGet]
    [Route(WebRoutes.GetStatusById)]
    public async Task<string> GetBookingStatusById([FromRoute] long id)
    {
        return await _bookingsQueries.GetStatusById(id, _cancellationToken);
    }
}