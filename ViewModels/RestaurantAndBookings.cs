using SGV_Booking.Models;

namespace SGV_Booking.ViewModels
{
    public class RestaurantAndBookings
    {
        public Restaurant TheRestaurant { get; set; }
        public List<Booking> RestaurantBookings { get; set; }

        public List<User> TheUsers { get; set; }

        public List<Banquet> TheBanquets { get; set; }
    }
}
