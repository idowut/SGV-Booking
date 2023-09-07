using System;
using System.Collections.Generic;

namespace SGV_Booking.Models
{
    public partial class Address
    {
        public Address()
        {
            Restaurants = new HashSet<Restaurant>();
        }

        public int AddressId { get; set; }
        public string AddressLine { get; set; } = null!;
        public string Suburb { get; set; } = null!;
        public string Postcode { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Country { get; set; } = null!;

        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
