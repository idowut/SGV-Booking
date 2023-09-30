using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGV_Booking.Data;
using SGV_Booking.Models;

namespace SGV_Booking.Controllers
{
    public class BookListingsController : Controller
    {
        const string SessionUserId = "_UserID";
        const string SessionUserName = "_UserName";

        private readonly SGVContext _context;

        public BookListingsController(SGVContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var SGVDatabaseContext = _context.Bookings.Include(b => b.Customer).Include(b => b.Restaurant);
            return View(await SGVDatabaseContext.ToListAsync());
        }

        // GET: Bookings/Details/5
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

        // GET: Bookings/CustomerDetails/5
        public async Task<IActionResult> CustomerDetails(int? id)
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
        public async Task<IActionResult> Create([Bind("BookingId,RestaurantId,BookingTime,CustomerId,BookingNotes,BanquetOption")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Users, "UserId", "UserId", booking.CustomerId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "RestaurantId", "RestaurantId", booking.RestaurantId);
            return View(booking);
        }

        public async Task<IActionResult> CustomerEdit(int id)
        {
            Console.WriteLine(id);
            if (id == null || _context.Bookings == null)
            {
                Console.WriteLine("im in here");
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Restaurant)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                Console.WriteLine("Im in here now");
                return NotFound();
            }

            Console.WriteLine(booking.BookingId);
            Console.WriteLine(booking.BookingNotes);

            Console.WriteLine("test");
            return View(booking);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerEdit(int id, Booking booking)
        {
            Console.WriteLine(booking.CustomerId);
            Console.WriteLine(booking.RestaurantId);
            Console.WriteLine(booking.BookingId);
            Console.WriteLine(booking.BookingNotes);
            Console.WriteLine(booking.BanquetOption);

            //type what you wanna say
            //now I can see customerID, restrantId on terminal but ModelState is still not valid.

            Console.WriteLine(id);
            if (id != booking.BookingId)
            {
                Console.WriteLine("Test0");
                return NotFound();
            }

            //foreach (var modelStateKey in ModelState.Keys)
            //{
            //    var modelStateVal = ModelState[modelStateKey];
            //    foreach (var error in modelStateVal.Errors)
            //    {
            //        Console.WriteLine(error.ErrorMessage);
            //    }
            //}

            try
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(booking.BookingId))
                {
                    Console.WriteLine("Test");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Console.WriteLine("Im here 1");
            return RedirectToAction("CustomerDetails", "BookListings", new { id = booking.BookingId });

        }
        public async Task<IActionResult> Delete(int? id)
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

            return View("Delete", booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'SGVDatabaseContext.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            var userId = booking.CustomerId;
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("CustomerIndex", "Users", new { id = userId });
        }

        private bool BookingExists(int id)
        {
            return (_context.Bookings?.Any(e => e.BookingId == id)).GetValueOrDefault();
        }
    }
}
