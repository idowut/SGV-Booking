namespace SGV_Booking.ViewModels
{
    public class BookingInfoProcess
    {
        public int restaurantID { get; set; }
        public DateTime bookingTime { get; set; }
        public int customerID { get; set; }
        public string bookingNotes { get; set; }

        public int? banquetOption { get; set; }

        public int numGuest { get; set; }

    }
}
