using SGV_Booking.Models;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace SGV_Booking.ViewModels
{
    public class UsersAndBookings
    {
        public User TheUser { get; set; }
        public Booking UserBookings { get; set; }
    }
}
