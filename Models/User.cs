using System;
using System.Collections.Generic;

namespace SGV_Booking.Models
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
        }

        public int UserId { get; set; }
        public int UserType { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int BookingsCount { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string? priorityStatus { get; set; } 

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
