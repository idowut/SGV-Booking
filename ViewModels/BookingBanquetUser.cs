using SGV_Booking.Models;

namespace SGV_Booking.ViewModels
{
    public class BookingBanquetUser
    {
        public Booking TheBooking { get; set; }

        public Banquet TheBanquet { get; set; }

        public User TheUser { get; set; }

        public Restaurant TheRestaurant { get; set; }
    }
}
