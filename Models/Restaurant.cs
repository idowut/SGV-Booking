using System;
using System.Collections.Generic;

namespace SGV_Booking.Models
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            Banquets = new HashSet<Banquet>();
            Bookings = new HashSet<Booking>();
        }

        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
        public int? RestaurantAddressId { get; set; }
        public string? RestaurantPhoneNumber { get; set; }

        public int RestaurantRewardValue { get; set; }

        public virtual Address? RestaurantAddress { get; set; }
        public virtual ICollection<Banquet> Banquets { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
