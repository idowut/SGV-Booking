using Microsoft.AspNetCore.Mvc;
using SGV_Booking.Models;
using System.Diagnostics;

namespace SGV_Booking.Controllers
{
    public class HomeController : Controller
    {
        const string SessionUserId = "_UserID";
        const string SessionUserName = "_UserName";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}