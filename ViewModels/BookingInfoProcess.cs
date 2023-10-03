namespace SGV_Booking.ViewModels
{
    public class BookingInfoProcess
    {
        public int restaurantSelect { get; set; }
        public string datePicker { get; set; }
        public TimeSpan timePicker { get; set; }
        public DateOnly date { get; set; }
        public int? customerID { get; set; }
        public string customerFirstName { get; set; }
        public string customerLastName { get; set; }
        public string customerPhone { get; set; }
        public string customerEmail { get; set; }
        public string bookingNotes { get; set; }

        public int? banquetOption { get; set; }

        public int guestNumber { get; set; }

    }
}
