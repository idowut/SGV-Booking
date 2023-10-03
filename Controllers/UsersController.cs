﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGV_Booking.Data;
using SGV_Booking.Models;
using SGV_Booking.ViewModels;

namespace SGV_Booking.Controllers
{
    public class UsersController : Controller
    {
        const string SessionUserId = "_UserID";
        const string SessionUserName = "_UserName";
        private readonly SGVContext _context;

        public UsersController(SGVContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'SGVDatabaseContext.Users'  is null.");
        }

        public async Task<IActionResult> RestaurantIndex(int id)
        {
            var vm = new RestaurantAndBookings();
            if (id == null || _context.Users == null)
            {
                Console.WriteLine("test");
                return NotFound();
            }
            vm.TheUsers = await _context.Users.ToListAsync();
            vm.TheRestaurant = await _context.Restaurants.FirstOrDefaultAsync(m => m.RestaurantId == id);
            vm.TheBanquets = await _context.Banquets.ToListAsync();
            vm.RestaurantBookings = await _context.Bookings
                .Where(i => i.RestaurantId == id)
                .OrderBy(i => i.BookingTime)
                .ToListAsync();

            if (vm.TheRestaurant == null)
            {
                return NotFound();
            }

            return View(vm);

        }


        // GET: Users/RestaurantBookingDetails/10004
        public async Task<IActionResult> RestaurantBookingDetails(int id)
        {
            var vm = new BookingBanquetUser();

            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            vm.TheBooking = await _context.Bookings.FirstOrDefaultAsync(i => i.BookingId == id);
            vm.TheBanquet = await _context.Banquets.FirstOrDefaultAsync(i => i.BanquetId == vm.TheBooking.BanquetOption);
            vm.TheUser = await _context.Users.FirstOrDefaultAsync(i => i.UserId == vm.TheBooking.CustomerId);
            vm.TheRestaurant = await _context.Restaurants.FirstOrDefaultAsync(i => i.RestaurantId == vm.TheBooking.RestaurantId);
            if (vm.TheBooking == null)
            {
                return NotFound();
            }

            return View(vm);
        }


        public async Task<IActionResult> CustomerIndex(UsersAndBookings vm, int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            vm.TheUser = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            vm.UserBookings = await _context.Bookings
                .Where(i => i.CustomerId == id)
                .Include(i => i.Restaurant)
                .OrderBy(i => i.BookingTime)
                .ToListAsync();

            if (vm.TheUser == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerIndex(int id, UsersAndBookings vm)
        {
            Console.WriteLine(id);
            Console.WriteLine(vm.TheUser.UserId);

            Console.WriteLine(vm.TheUser.FirstName);

            if (id != vm.TheUser.UserId)
            {
                return NotFound();
            }


            Console.WriteLine("Im here 0");
            try
            {
                _context.Update(vm.TheUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("It caught an error");
                if (!UserExists(vm.TheUser.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            vm.TheUser = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            vm.UserBookings = await _context.Bookings
                .Where(i => i.CustomerId == id)
                .Include(i => i.Restaurant)
                .ToListAsync();

            Console.WriteLine("Im here 1");
            return View(vm);

            Console.WriteLine("Im here 2");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register([Bind("UserID, FirstName, LastName, PhoneNumber, Email, Password")] User user)
        {
            ViewBag.phoneError = null;
            ViewBag.emailError = null;
            ViewBag.firstName = user.FirstName;


            if (ModelState.IsValid)
            {
                var registerQuery = _context.Users
                    .Where(userindata => userindata.PhoneNumber.Equals(user.PhoneNumber) || userindata.Email.Equals(user.Email))
                    .Select(userindata => userindata)
                    .ToList();


                if (!user.Email.Contains("@"))
                {
                    ViewBag.emailError = "Incorrect Format - e.g name@example.com";
                }

                if (!user.PhoneNumber.All(char.IsDigit) || user.PhoneNumber.Length < 10)
                {
                        ViewBag.phoneError = "Invalid Phone Number - e.g. 0123456789";
                }


                if (registerQuery.Count == 0 && user.Email.Contains("@") && user.PhoneNumber.All(char.IsDigit) && user.PhoneNumber.Length == 10)
                {
                    user.UserType = 2;
                    user.BookingsCount = 0;
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    Console.WriteLine(user);
                    return RedirectToAction(nameof(RegisterDetails));
                }

               

                if (registerQuery.Count > 0)
                {
                    var userEmailAndPassword = await _context.Users
                        .FirstOrDefaultAsync(userindata => userindata.PhoneNumber.Equals(user.PhoneNumber) || userindata.Email.Equals(user.Email));

                    if (userEmailAndPassword.PhoneNumber == user.PhoneNumber)
                    {
                        ViewBag.phoneError = "Phone number is already in use.";
                    }
                    if (userEmailAndPassword.Email == user.Email)
                    {
                        ViewBag.emailError = "Email is already in use.";
                    }

                }


                return View();
            }

            return View();
        }


        public IActionResult RegisterDetails()
        {
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email, Password")] User user)
        {
            ViewBag.noLogin = null;

            //If the enters their login, then they are sent to the index page.
            ViewBag.num = null;
            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                //If user input is in the database.
                var login = _context.Users
                    .FirstOrDefault(userindata => userindata.Email.Equals(user.Email) && userindata.Password.Equals(user.Password));

                if (login == null)
                {
                    if (!string.IsNullOrWhiteSpace(user.Password))
                    {
                        ViewBag.noLogin = "Incorrect Login. Try checking your email or password.";
                    }
                }

                if (login != null)
                {
                    var loggedUser = _context.Users.FirstOrDefault(userindata => userindata.Email.Equals(user.Email));
                    Console.WriteLine("error is here");

                    if (loggedUser.UserType == 0)
                    {
                        return RedirectToAction("AdminIndex", new { id = loggedUser.UserId });
                    }

                    else if (loggedUser.UserType == 1)
                    {

                        var restaurant = await _context.Restaurants
                            .FirstOrDefaultAsync(i => i.RestaurantName == loggedUser.FirstName);
                        HttpContext.Session.SetInt32(SessionUserId, restaurant.RestaurantId);
                        HttpContext.Session.SetString(SessionUserName, loggedUser.FirstName);

                        return RedirectToAction("RestaurantIndex", new { id = restaurant.RestaurantId });
                    }

                    else if (loggedUser.UserType == 2)
                    {
                        HttpContext.Session.SetInt32(SessionUserId, loggedUser.UserId);
                        HttpContext.Session.SetString(SessionUserName, loggedUser.FirstName);

                        return RedirectToAction("CustomerIndex", new { id = loggedUser.UserId });
                    }
                }
            }
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.SetInt32(SessionUserId, -1);
            HttpContext.Session.SetString(SessionUserName, "");
            return View();
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Restaurant)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost]
        public IActionResult ChangeBookingStatus(int bookingId)
        {

            var booking = _context.Bookings.SingleOrDefault(b => b.BookingId == bookingId);

            if (booking != null)
            {
                int resturantID = booking.RestaurantId;

                booking.BookingStatus = true;

                UpdateBooking(booking);

                return RedirectToAction("RestaurantIndex", new { id = resturantID });

            }

            return RedirectToAction("RestaurantIndex");
        }

        private void UpdateBooking(Booking booking)
        {
            var existingBooking = _context.Bookings.SingleOrDefault(b => b.BookingId == booking.BookingId);

            if (existingBooking != null)
            {
                existingBooking.BookingStatus = booking.BookingStatus;

                _context.Entry(existingBooking).State = EntityState.Modified;

                _context.SaveChanges();
            }
            else
            {
                return;
            }
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
