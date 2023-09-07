using Microsoft.AspNetCore.Mvc;
using SGV_Booking.Models;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace SGV_Booking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }       

        public IActionResult RegisterDetails(string passwordRegister, string emailRegister)
        {
            ViewBag.password = passwordRegister;
            ViewBag.email = emailRegister;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}