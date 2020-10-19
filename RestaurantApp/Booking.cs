using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace RestaurantApp
{
    class Booking
    {

        //MODEL (MVC)

        private int bookingID;
        private int restaurantID;
        private int customerID;
        private DateTime bookingDate;
        private int bookingTime;

        //constructor - new booking

        public Booking(int restaurantID, DateTime bookingDate, int bookingTime, int customerID)
        {

            //set member variables
            this.bookingID = GetNewBookingID();
            this.restaurantID = restaurantID;
            this.customerID = customerID;
            this.bookingDate = bookingDate;
            this.bookingTime = bookingTime;
            //BookTable(restaurantID, bookingDate, customerID);

        }

        //constructor - existing bookings (SQL)
        public Booking(int bookingID, int restaurantID, DateTime bookingDate, int customerID)
        {

            //set member variables
            this.bookingID = bookingID;
            this.restaurantID = restaurantID;
            this.customerID = customerID;
            this.bookingDate = bookingDate;



        }

        private int GetNewBookingID()
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);
            object scalarVal;
          

            int newBookingID;
            sqlCmd.CommandText = "select MAX(book_ID) from tbl_Bookings";
            sqlCmd.Connection = sqlConnection;

            sqlConnection.Open();
            scalarVal = sqlCmd.ExecuteScalar();

            if (scalarVal == null || scalarVal == System.DBNull.Value)
            {
                newBookingID = 0;

            }
            else
            {
                newBookingID = (int)sqlCmd.ExecuteScalar();


            }


            sqlConnection.Close();
            return newBookingID + 1;

        }

        public void BookTable(int restID, string bookingDate, int customerID, int bookTime)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            //create Insert command
            sqlCmd.CommandType = System.Data.CommandType.Text;

            sqlCmd.CommandText = "INSERT INTO tbl_Bookings (book_restID, book_date, book_time, book_custID) VALUES (@param1, @param2, @param3, @param4)";
            sqlCmd.Parameters.AddWithValue("@param1", restID);
            sqlCmd.Parameters.AddWithValue("@param2", bookingDate);
            sqlCmd.Parameters.AddWithValue("@param3", bookTime);
            sqlCmd.Parameters.AddWithValue("@param4", customerID);

            sqlCmd.Connection = sqlConnection;

            sqlConnection.Open();
            sqlCmd.ExecuteNonQuery();

            sqlConnection.Close();

        }

        public void CancelBooking(int bookingID)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            //create Insert command
            sqlCmd.CommandType = System.Data.CommandType.Text;

            sqlCmd.CommandText = "DELETE FROM tbl_Bookings WHERE booking_ID = @param1";

            sqlCmd.Parameters.AddWithValue("@param1", bookingID);
            sqlCmd.Connection = sqlConnection;


            sqlConnection.Open();
            sqlCmd.ExecuteNonQuery();

            sqlConnection.Close();
        }

        public void UpdateBooking(int bookingID, DateTime bookingDate)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            //create Insert command
            sqlCmd.CommandType = System.Data.CommandType.Text;

            sqlCmd.CommandText = "UPDATE tbl_Bookings SET booking_dateTime = @param1 WHERE rest_ID = @param0";
            sqlCmd.Parameters.AddWithValue("@param0", bookingID);
            sqlCmd.Parameters.AddWithValue("@param1", bookingDate);


            sqlCmd.Connection = sqlConnection;


            sqlConnection.Open();
            sqlCmd.ExecuteNonQuery();

            sqlConnection.Close();
        }


        public List<Booking> GetBookings()
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            List<Booking> books = new List<Booking>();

            string theQueryString = "select * from tbl_Bookings";

            using (sqlConnection)
            {


                SqlCommand sqlCommand =
                        new SqlCommand(theQueryString, sqlConnection);
                sqlConnection.Open();



                SqlDataReader sqlReader = sqlCommand.ExecuteReader();



                while (sqlReader.Read())
                {


                    books.Add(new Booking(sqlReader.GetInt32(0), sqlReader.GetInt32(1), Convert.ToDateTime(sqlReader.GetString(2)), sqlReader.GetInt32(3)));
                    
                }


                sqlReader.Close();
            }

            sqlConnection.Close();

            return books;
        }

        public List<Booking> GetBookings(int restID)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            List<Booking> books = new List<Booking>();

            string theQueryString = "select * from tbl_Bookings where booking_resaurantID = '" + restID + "'";

            using (sqlConnection)
            {


                SqlCommand sqlCommand =
                        new SqlCommand(theQueryString, sqlConnection);
                sqlConnection.Open();



                SqlDataReader sqlReader = sqlCommand.ExecuteReader();



                while (sqlReader.Read())
                {


                    books.Add(new Booking(sqlReader.GetInt32(0), sqlReader.GetInt32(1), Convert.ToDateTime(sqlReader.GetString(2)), sqlReader.GetInt32(3)));

                }


                sqlReader.Close();
            }

            sqlConnection.Close();

            return books;
        }

        public List<int> GetTimes(int restID, string aDate, int tableCount)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            List<int> bookTimes = new List<int>();

            string theQueryString = "SELECT book_time FROM tbl_Bookings WHERE book_date = '" + aDate + "' and book_restID = '" + restID + "' GROUP BY book_time HAVING COUNT(book_time) = '" + tableCount + "';";

           

            using (sqlConnection)
            {


                SqlCommand sqlCommand =
                        new SqlCommand(theQueryString, sqlConnection);
                sqlConnection.Open();



                SqlDataReader sqlReader = sqlCommand.ExecuteReader();



                while (sqlReader.Read())
                {


                    bookTimes.Add(sqlReader.GetInt32(0));

                }


                sqlReader.Close();
            }

            sqlConnection.Close();

            return bookTimes;
        }


        public List<string> GetBookings(int restID, int userID)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            List<string> books = new List<string>();

            //filter on role (if userID = 0 then the role is Owner)
            string filter = "";

            if(userID != 0)
            {
                filter = " and a.acct_ID = '" + userID + "';";
            }

            string theQueryString = "select r.rest_name, a.acct_username, b.book_date, b.book_time, b.book_ID from tbl_Bookings b inner join tbl_Accounts a ON b.book_custID = a.acct_ID inner join tbl_Restaurants r on b.book_restID = r.rest_ID where r.rest_ID = '" + restID + "'" + filter;

            using (sqlConnection)
            {


                SqlCommand sqlCommand =
                        new SqlCommand(theQueryString, sqlConnection);
                sqlConnection.Open();



                SqlDataReader sqlReader = sqlCommand.ExecuteReader();



                while (sqlReader.Read())
                {


                    books.Add(sqlReader.GetString(0) + "," + sqlReader.GetString(1) + "," + sqlReader.GetDateTime(2).ToString() + "," + sqlReader.GetInt32(3).ToString() + "," + sqlReader.GetInt32(4).ToString());

                }


                sqlReader.Close();
            }

            sqlConnection.Close();

            return books;
        }
    }
}
