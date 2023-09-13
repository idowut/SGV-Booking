using Microsoft.AspNetCore.Mvc;

namespace SGV_Booking.Controllers
{
    public class BookingController : Controller
    {
        const string SessionUserId = "_UserID";
        const string SessionUserName = "_UserName";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Booking()
        {
            return View();
        }
        public IActionResult BookingSelection()
        {
            return View();
        }
        public IActionResult BookingSummary()
        {
            return View();
        }
    }
}
