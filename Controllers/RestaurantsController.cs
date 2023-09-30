using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGV_Booking.Data;

namespace SGV_Booking.Controllers
{
    public class RestaurantsController : Controller
    {
        const string SessionUserId = "_UserID";
        const string SessionUserName = "_UserName";

        private readonly SGVContext _context;

        public RestaurantsController(SGVContext context)
        {
            _context = context;
        }
        private IActionResult GetRestaurantView(string restaurantName)
        {
            var restaurant = _context.Restaurants
                                      .Include(r => r.RestaurantAddress)
                                      .FirstOrDefault(r => r.RestaurantName == restaurantName);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        public IActionResult Bamboo() => GetRestaurantView("Bamboo Leaf");
        public IActionResult La_oeste() => GetRestaurantView("La Oeste De La Mar");
        public IActionResult Mexikana() => GetRestaurantView("Mexikana");

    }
}
