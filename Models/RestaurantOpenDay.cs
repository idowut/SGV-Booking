using System;
using System.Collections.Generic;

namespace SGV_Booking.Models
{
    public partial class RestaurantOpenDay
    {
        public int RestaurantId { get; set; }
        public bool OpenMonday { get; set; }
        public bool OpenTuesday { get; set; }
        public bool OpenWednesday { get; set; }
        public bool OpenThursday { get; set; }
        public bool OpenFriday { get; set; }
        public bool OpenSaturday { get; set; }
        public bool OpenSunday { get; set; }

        public virtual Restaurant Restaurant { get; set; } = null!;
    }
}
