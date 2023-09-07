using System;
using System.Collections.Generic;

namespace SGV_Booking.Models
{
    public partial class BanquetItem
    {
        public int BanquetItemId { get; set; }
        public int BanquetId { get; set; }
        public string BanquetItem1 { get; set; } = null!;
        public string? BanquetDesc { get; set; }

        public virtual Banquet Banquet { get; set; } = null!;
    }
}
