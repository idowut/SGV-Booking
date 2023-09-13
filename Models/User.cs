using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Please Enter your First Name.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter your Last Name.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter your Email.")]
        public string Email { get; set; } = null!;
        public int BookingsCount { get; set; }

        [Required(ErrorMessage = "Please Enter your Phone Number")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter a Password.")]
        public string Password { get; set; } = null!;

        public Boolean? priorityStatus { get; set; } 

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
