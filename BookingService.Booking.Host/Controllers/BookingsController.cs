using Microsoft.AspNetCore.Mvc;
using BookingService.Booking.AppServices.Bookings;

namespace BookingService.Booking.Host.Controllers
{
    public class BookingsController : Controller
    {
        private IBookingsService _bookingsService;
        private IBookingsQueries _bookingsQueries;
        public BookingsController(IBookingsService bookingsService, IBookingsQueries bookingsQueries)
        {
            _bookingsService = bookingsService;
            _bookingsQueries = bookingsQueries;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
