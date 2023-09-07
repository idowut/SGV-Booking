using System;
using System.Collections.Generic;

namespace SGV_Booking.Models
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime BookingTime { get; set; }
        public int CustomerId { get; set; }
        public string? BookingNotes { get; set; }
        public int? BanquetOption { get; set; }

        public virtual User Customer { get; set; } = null!;
        public virtual Restaurant Restaurant { get; set; } = null!;
    }
}
