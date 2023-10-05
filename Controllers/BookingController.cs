using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGV_Booking.Data;
using SGV_Booking.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Localization;
using System.Net;
using System.Net.Mail; 
using System.Threading.Tasks; 

namespace SGV_Booking.Controllers
{
    public class BookingController : Controller
    {
        const string SessionUserId = "_UserID";
        const string SessionUserName = "_UserName";
        private readonly SGVContext _context;
        private readonly IConfiguration connString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingController(SGVContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            connString = configuration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;  // Assigning the injected instance to the field
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

            if (!TimeSpan.TryParse(timeString, out TimeSpan time))
            {
                // Handle the error here, maybe return to the same view with a error message
                ViewBag.ErrorMessage = "Time is in an invalid format.";
                return View("Error"); // Or return the same view with an error message.
            }

            vm.customerID = -1;

            if (vm.restaurantSelect == 0)
            {
                return View("BookingSelection");
            }

            return View("BookingSelection", vm);
        }

        [HttpPost]
        public ActionResult Booking(BookingInfoProcess vm)
        {
            ViewBag.banquet = vm.banquetOption;
            if (vm.restaurantSelect == 0)
            {

                vm.ErrorMessage = "Please Select a Restaurant...";
                ViewBag.RestaurantsList = _context.Restaurants.Select(r => new SelectListItem
                {
                    Value = r.RestaurantId.ToString(),
                    Text = r.RestaurantName,
                })
           .ToList();

                return View("BookingSelection", vm);
            }

            if (!vm.banquetOption.HasValue)
            {
                vm.ErrorMessage = "Pleast Select a Banquet...";
                ViewBag.RestaurantsList = _context.Restaurants.Select(r => new SelectListItem
                {
                    Value = r.RestaurantId.ToString(),
                    Text = r.RestaurantName,
                })
                .ToList();

                return View("BookingSelection", vm);

            }

            if (string.IsNullOrEmpty(vm.datePicker))
            {
                vm.ErrorMessage = "Please Select a Date";
                ViewBag.date = vm.datePicker;
                ViewBag.RestaurantsList = _context.Restaurants.Select(r => new SelectListItem
                {
                    Value = r.RestaurantId.ToString(),
                    Text = r.RestaurantName,
                })
                .ToList();

                return View("BookingSelection", vm);
            }

            if(vm.timePicker == TimeSpan.Zero)
            {
                vm.ErrorMessage = "Please Select a Time...";
                ViewBag.date = vm.datePicker;
                ViewBag.RestaurantsList = _context.Restaurants.Select(r => new SelectListItem
                {
                    Value = r.RestaurantId.ToString(),
                    Text = r.RestaurantName,
                })
                .ToList();

                return View("BookingSelection", vm);
            }

            return View("Booking", vm);
        }
        [HttpPost]
        public IActionResult BookingSummary(BookingInfoProcess vm)
        {
            ViewBag.emailError = null;
            ViewBag.phoneError = null;

            if (!vm.customerEmail.Contains("@"))
            {
                ViewBag.emailError = "Please Enter the Correct Format - e.g. name@example.com";
                return View("Booking", vm);
            }

            if(!vm.customerPhone.All(char.IsDigit))
            {
                ViewBag.phoneError = "Invalid Phone Number";
            }

            return View("BookingSummary", vm);
        }








        public async Task<IActionResult> BookingConfirmation(BookingInfoProcess vm)
        {
            DateTime dt = Convert.ToDateTime(vm.datePicker + " " + vm.timePicker);

            var Restaurant = await _context.Restaurants.FirstOrDefaultAsync(i => i.RestaurantId == vm.restaurantSelect);
            int RestaurantId = vm.restaurantSelect;

            var BookingNotes = vm.bookingNotes;

            if (BookingNotes == null || BookingNotes.Length == 0 || BookingNotes == "")
            {
                BookingNotes = "No Booking Notes Provided...";
            }

            var BanquetOption = vm.banquetOption;

            var NumGuest = vm.guestNumber;

            string query = "INSERT INTO Bookings (restaurantID, bookingTime, customerID, bookingNotes, numGuest, bookingStatus)";
            query += "VALUES(@RestaurantID, @dateTime, @CustomerID, @BookingNotes, @NumGuest, @bookingStatus)";

            string customerQuery = "INSERT INTO Users (userType, firstName, lastName, email, phoneNumber, password, bookingsCount)";
            customerQuery += "VALUES(@userType, @firstname, @lastname, @email, @phoneNumber, @password, @bookCount)";

            Console.WriteLine(connString.ToString());

            SqlConnection connectionString = new SqlConnection(connString.GetConnectionString("SGVContext"));

            connectionString.Open();

            var Customer = await _context.Users.FirstOrDefaultAsync(i => i.FirstName == vm.customerFirstName && i.LastName == vm.customerLastName && i.Email == vm.customerEmail);

            if (Customer == null)
            {
                SqlCommand customerCommand = new SqlCommand(customerQuery, connectionString);
                customerCommand.Parameters.AddWithValue("@userType", 2);
                customerCommand.Parameters.AddWithValue("@firstname", vm.customerFirstName);
                customerCommand.Parameters.AddWithValue("@lastname", vm.customerLastName);
                customerCommand.Parameters.AddWithValue("@email", vm.customerEmail);
                customerCommand.Parameters.AddWithValue("@phoneNumber", vm.customerPhone);
                customerCommand.Parameters.AddWithValue("@password", "aaaaaaaaaaaaaaaaaaaa");
                customerCommand.Parameters.AddWithValue("@bookCount", 1);
                int j = customerCommand.ExecuteNonQuery();
            }

            var Customer2 = await _context.Users.FirstOrDefaultAsync(i => i.FirstName == vm.customerFirstName && i.LastName == vm.customerLastName && i.Email == vm.customerEmail);
            var CustomerId = Customer2.UserId;

            SqlCommand addCommand = new SqlCommand(query, connectionString);
            addCommand.Parameters.AddWithValue("@RestaurantID", RestaurantId);
            addCommand.Parameters.AddWithValue("@dateTime", dt);
            addCommand.Parameters.AddWithValue("@CustomerID", CustomerId);
            addCommand.Parameters.AddWithValue("@BookingNotes", BookingNotes);
            addCommand.Parameters.AddWithValue("@NumGuest", NumGuest);
            addCommand.Parameters.AddWithValue("@bookingStatus", 0);

            int i = addCommand.ExecuteNonQuery();

            connectionString.Close();

            try
            {
                var restaurant = _context.Restaurants.Where(r => r.RestaurantId == vm.restaurantSelect).FirstOrDefault();
                var fromEmail = "SGVBOOKINGS@outlook.com"; // Replace with your email address
                var fromEmailPassword = "test345";
                var toEmail = vm.customerEmail;
                var subject = "Booking Confirmation";
                var body = $@"Thank you for booking with us! Your booking has been confirmed. <br><br>
Booking Details:  <br><br>
Date and Time: {vm.datePicker} {vm.timePicker}  <br><br>
Restaurant: {restaurant.RestaurantName}  <br><br>
Booking Notes: {vm.bookingNotes}  <br><br>
Number of Guests: {vm.guestNumber}";

                using (var smtpClient = new SmtpClient("smtp.office365.com"))
                {
                    smtpClient.Port = 587; // Replace with the SMTP server port of your email provider
                    smtpClient.Credentials = new NetworkCredential(fromEmail, fromEmailPassword); // Replace with your email password
                    smtpClient.EnableSsl = true; // Use SSL if your email provider requires it

                    var mailMessage = new MailMessage(fromEmail, toEmail, subject, body);
                    mailMessage.IsBodyHtml = true; // You can use HTML in the email body if needed

                    await smtpClient.SendMailAsync(mailMessage);

                    Console.WriteLine("Email sent successfully"); // Add this line

                }

                ViewBag.EmailSent = true;
            }
            catch (Exception ex)
            {
                ViewBag.EmailSent = false;
                ViewBag.EmailError = ex.Message;
                Console.WriteLine("Email sending error: " + ex.ToString());
            }

            vm.datePicker = vm.datePicker;
            var dateConversion = vm.datePicker;
            if (_httpContextAccessor.HttpContext?.Session.GetInt32("_UserID") == null)
            {
                vm.customerID = -1;
            }
            else
            {
                vm.customerID = _httpContextAccessor.HttpContext?.Session.GetInt32("_UserID");
            }

            if (i != 0)
            {
                return View("BookingConfirmation", vm);
            }
            else
            {
                Console.WriteLine("not successful");
                return View("BookingConfirmation", vm);
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

        public IActionResult bookingLists(int id)
        {
            var apiData = _context.Bookings.Where(b => b.RestaurantId == id).ToList();

            return Json(apiData);
        }

        public IActionResult GetUserId()
        {
            var userId = HttpContext.Session.GetInt32(SessionUserId);

            if (userId == null)
            {
                userId = -1;
            }

            return Json(userId);
        }

        public IActionResult GetUserInfo(int id)
        {
            var apiData = _context.Users.Where(user => user.UserId == id).ToList();
            return Json(apiData);
        }

    }
}
