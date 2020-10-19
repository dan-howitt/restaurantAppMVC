using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp
{
    class ControlBooking
    {
        private Booking newBooking;

        public ControlBooking(int restaurantID, DateTime bookingDate, int bookingTime, int customerID)
        {

            newBooking = new Booking(restaurantID, bookingDate, bookingTime, customerID);


        }

     

        public List<string> GetBookings(int restID, int userID)
        {
            return newBooking.GetBookings(restID, userID);
        }

        public List<int> GetTimes(int restID, string aDate, int tableCount)
        {
            return newBooking.GetTimes(restID, aDate, tableCount);
        }

        public void BookTable(int restID, string bookingDate, int customerID, int bookTime)
        {
            newBooking.BookTable(restID, bookingDate, customerID, bookTime);
        }
    }
}
