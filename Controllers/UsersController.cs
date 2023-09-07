using Microsoft.AspNetCore.Mvc;

namespace SGV_Booking.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult RegisterDetails(string passwordRegister, string emailRegister, string firstName, string lastName, string addressRegister)
        {
            ViewBag.password = passwordRegister;
            ViewBag.email = emailRegister;
            ViewBag.firstName = firstName;
            ViewBag.lastName = lastName;
            ViewBag.address = addressRegister;

            return View();
        }
    }
}
