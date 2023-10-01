using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        public IActionResult RestaurantOpenTimes(int id)
        {
            return View();
        }

    }
}
