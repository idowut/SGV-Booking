using Microsoft.AspNetCore.Mvc;
using SGV_Booking.Models;

namespace SGV_Booking.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Booking()
        {
            return View();
        }
    }
}
