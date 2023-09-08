using Microsoft.AspNetCore.Mvc;

namespace SGV_Booking.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Register()
        {
            return View();
        }
    }
}
