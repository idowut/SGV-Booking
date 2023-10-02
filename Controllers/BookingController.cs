using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGV_Booking.Data;
using SGV_Booking.Models;
using SGV_Booking.ViewModels;

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
        public IActionResult BookingSelection()
        {
            ViewBag.RestaurantsList = _context.Restaurants.Select(r => new SelectListItem
            {
                Value = r.RestaurantId.ToString(),
                Text = r.RestaurantName,
            })
            .ToList();

            var vm = new BookingInfoProcess();

            return View(vm);
        }

        [HttpPost]
        public ActionResult BookingSelection(BookingInfoProcess vm, string timeString)
        {
            TimeSpan time;
            bool isTimeParseSuccessful = TimeSpan.TryParse(timeString, out time);

            DateTime date = vm.datePicker;
            vm.datePicker = date;

            vm.timePicker = time;
            return View("Booking", vm); // Pass it as a parameter to the next step
        }
        [HttpPost]
        public ActionResult Booking(BookingInfoProcess vm)
        {
            return View("Booking", vm);
        }
        [HttpPost]
        public IActionResult BookingSummary(BookingInfoProcess vm)
        {
            return View("BookingSummary", vm);
        }
        public async Task<IActionResult> BookingConfirmation(int guestCount, string date, string bookingTime, string selectedRestaurant, string selectedBanquet, string firstName, string lastName, string email, string phoneNumber, string dietaryRequirements)
        {
            DateTime dt = Convert.ToDateTime(date + " " + bookingTime);

            Console.WriteLine(selectedRestaurant);
            var Restaurant = await _context.Restaurants.FirstOrDefaultAsync(i => i.RestaurantName == selectedRestaurant);
            var RestaurantId = Restaurant.RestaurantId;

            var Customer = await _context.Users.FirstOrDefaultAsync(i => i.FirstName == firstName && i.LastName == lastName && i.Email == email); ;

            var CustomerId = -1;
            if (Customer != null)
            {
                CustomerId = Customer.UserId;
            }

            var BookingNotes = dietaryRequirements;

            var BanquetOption = selectedBanquet;

            var NumGuest = guestCount;

            string query = "INSERT INTO Bookings (restaurantID, bookingTime, customerID, bookingNotes, numGuest)";
            query += "VALUES(@RestaurantID, @dateTime, @CustomerID, @BookingNotes, @NumGuest)";

            SqlConnection connectionString = new SqlConnection("Data Source=DESKTOP-8PR6E5F\\SQLEXPRESS;Initial Catalog=SGV;Integrated Security=True");

            SqlCommand addCommand = new SqlCommand(query, connectionString);
            addCommand.Parameters.AddWithValue("@RestaurantID", RestaurantId);
            addCommand.Parameters.AddWithValue("@dateTime", dt);
            addCommand.Parameters.AddWithValue("@CustomerID", CustomerId);
            addCommand.Parameters.AddWithValue("@BookingNotes", BookingNotes);
            addCommand.Parameters.AddWithValue("@NumGuest", NumGuest);

            connectionString.Open();
            int i = addCommand.ExecuteNonQuery();

            connectionString.Close();

            if (i != 0)
            {
                return View();
            }
            else
            {
                Console.WriteLine("not successful");
                return View();
            }
        }

        public IActionResult restaurantInfo(int id)
        {
            var apiData = _context.Restaurants.Where(r => r.RestaurantId == id)
                .Select(r => new
                {
                    r.RestaurantAddressId,
                    r.RestaurantName,
                    OpenTimes = _context.RestaurantOpenTimes.Where(r => r.RestaurantId == id).ToList(),
                    OpenDays = _context.RestaurantOpenDays.Where(r => r.RestaurantId == id).ToList(),
                    Banquets = _context.Banquets.Where(r => r.RestaurantId == id).Select(r => new
                    {
                        r.BanquetId,
                        r.BanquetName,
                        r.BanquetMinPeople,
                        r.BanquetPrice,
                        BanquetItems = _context.BanquetItems.Where(b => b.BanquetId == r.BanquetId).ToList(),
                    }).ToList(),
                })
                .ToList();

            return Json(apiData);
        }

    }
}
