using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGV_Booking.Data;

namespace SGV_Booking.Controllers
{
    public class BookingController : Controller
    {
        const string SessionUserId = "_UserID";
        const string SessionUserName = "_UserName";
        private readonly SGVContext _context;

        public BookingController(SGVContext context)
        {
            _context = context;
        }
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
        public IActionResult BookingConfirmation(int guestCount, DateTime dateTime, string bookingTime, string selectedRestaurant, string selectedBanquet, string firstName, string lastName, string email, string phoneNumber, string dietaryRequirements)
        {
            int booking_guestCount = guestCount;
            DateTime booking_dateTime = dateTime;
            string booking_bookingTime = bookingTime;
            string booking_selectedRestaurant = selectedRestaurant;
            string booking_selectedBanquet = selectedBanquet;
            string booking_firstName = firstName;
            string bookingLastName = lastName;
            string bookingEmail = email;
            string bookingPhoneNumber = phoneNumber; 
            string bookingDietaryRequirements = dietaryRequirements;

            Console.WriteLine(booking_firstName);
            
            Booking









            return View();
        }
    }
}
