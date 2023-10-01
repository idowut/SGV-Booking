using System;
using System.Collections.Generic;

namespace SGV_Booking.Models
{
    public partial class RestaurantOpenTime
    {
        public int TimeSlotId { get; set; }
        public int RestaurantId { get; set; }
        public string SlotName { get; set; } = null!;
        public TimeSpan SlotOpenTime { get; set; }
        public TimeSpan SlotCloseTime { get; set; }

        public virtual Restaurant Restaurant { get; set; } = null!;
    }
}
