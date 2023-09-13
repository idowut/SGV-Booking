using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGV_Booking.Data;
using SGV_Booking.Models;
using SGV_Booking.ViewModels;

namespace SGV_Booking.Controllers
{
    public class UsersController : Controller
    {
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
                .OrderByDescending(i => i.BookingTime)
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

        public async Task<IActionResult> Register([Bind("UserID, FirstName, LastName, PhoneNumber, Email, Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UserType = 2;
                user.BookingsCount = 0;
                _context.Add(user);
                await _context.SaveChangesAsync();
                Console.WriteLine(user);
                return RedirectToAction(nameof(RegisterDetails));
            }

            return View();
        }
        public IActionResult RegisterDetails()
        {
            return View();
        }

        public IActionResult Login(string emailLogin, string passwordLogin)
        {
            ViewBag.num = null;
            if (!string.IsNullOrWhiteSpace(emailLogin))
            {
                var loginQuery = _context.Users
                    .Where(user => user.Email.Equals(emailLogin) && user.Password.Equals(passwordLogin))
                    .Select(user => user)
                    .ToList();

                ViewBag.email = loginQuery.Count;
                if (loginQuery.Count > 0)
                {
                    var user = _context.Users.FirstOrDefault(user => user.Email.Equals(emailLogin));

                    return RedirectToAction("CustomerIndex", new {id = user.UserId});
                }
            }
            return View();
        }

        public IActionResult LoginDetails()
        {
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

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "RestaurantId", "RestaurantId");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserType,FirstName,LastName,Email,PhoneNumber,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserType,FirstName,LastName,Email,PhoneNumber,Password")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                Console.WriteLine("Im here 1");
                return RedirectToAction(nameof(Index));
            }
            Console.WriteLine("Im here 2");
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'SGVDatabaseContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
