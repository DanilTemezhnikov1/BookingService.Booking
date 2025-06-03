using BookingService.Booking.Api.Contracts.Bookings;
using BookingService.Booking.Api.Contracts.Bookings.Dtos;
using BookingService.Booking.Api.Contracts.Bookings.Requests;
using BookingService.Booking.AppServices.Bookings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Booking.Host.Controllers
{
  //  [Route(WebRoutes.BasePath)]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private IBookingsService _bookingsService;
        private IBookingsQueries _bookingsQueries;

        [HttpPost]
        [Route(WebRoutes.Create)]
        public long CreateBooking([FromBody]CreateBookingRequest createBookingRequest)
        {
            long? idUser = createBookingRequest.IdUser;
            long? idBooking = createBookingRequest.IdBooking;
            DateOnly? startBooking = createBookingRequest.StartBooking;
            DateOnly? endBooking = createBookingRequest.EndBooking;
            return _bookingsService.Create(idUser, idBooking, startBooking, endBooking);
        }

        [HttpPost]
        [Route(WebRoutes.GetByFilter)]
        public async Task<BookingData[]> GetBookingsByFilter([FromBody]GetBookingsByFilterRequest getBookingsByFilter)
        {
            BookingData.BookingStatus? status = (BookingData.BookingStatus)getBookingsByFilter.Status;
            long? idUser = getBookingsByFilter.IdUser;
            long? idBooking = getBookingsByFilter.IdBooking;
            DateOnly? startBooking = getBookingsByFilter.StartBooking;
            DateOnly? endBooking = getBookingsByFilter.EndBooking;
            return _bookingsQueries.GetByFilter(status, idUser, idBooking, startBooking, endBooking);
        }

        [HttpPost]
        [Route(WebRoutes.Cancel)]
        public async Task CancelBooking([FromBody]long? id)
        {
             _bookingsService.Cancel(id);
        }
        [HttpGet]
        [Route(WebRoutes.GetById)]
        public async Task<BookingData> GetBookingById([FromBody] long? id)
        {
            return _bookingsService.GetById(id);
        }
        [HttpGet]
        [Route(WebRoutes.GetStatusById)]
        public async Task<BookingData.BookingStatus> GetBookingStatusById([FromBody]long id)
        {
            return _bookingsQueries.GetStatusById(id);
        }
        public BookingsController(IBookingsService bookingsService, IBookingsQueries bookingsQueries)
        {
            _bookingsService = bookingsService;
            _bookingsQueries = bookingsQueries;
        }
    }
}
