using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGV_Booking.Models
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
        }


        [Key]
        public int UserId { get; set; }
        public int UserType { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = null!;
        public int BookingsCount { get; set; }

        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [StringLength(150, ErrorMessage = "Reached Password Length Limit.")]
        public string Password { get; set; } = null!;
        public bool? PriorityStatus { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
