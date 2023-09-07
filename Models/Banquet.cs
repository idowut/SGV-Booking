using System;
using System.Collections.Generic;

namespace SGV_Booking.Models
{
    public partial class Banquet
    {
        public Banquet()
        {
            BanquetItems = new HashSet<BanquetItem>();
        }

        public int BanquetId { get; set; }
        public int RestaurantId { get; set; }
        public string BanquetName { get; set; } = null!;
        public int BanquetMinPeople { get; set; }
        public int BanquetPrice { get; set; }

        public virtual Restaurant Restaurant { get; set; } = null!;
        public virtual ICollection<BanquetItem> BanquetItems { get; set; }
    }
}
