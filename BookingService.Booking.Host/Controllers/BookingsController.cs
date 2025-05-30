using Microsoft.AspNetCore.Mvc;

namespace BookingService.Booking.Host.Controllers
{
    public class BookingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
