using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace RestaurantApp
{
    //MODEL (MVC)
    class Restaurant
    {

        private int restaurantID;
        private string name;
        private string address;
        private string description;
        private string daysAvailable;
        private string openHours;
        private int restTables;


        //constructor - new restaurant 

        public Restaurant(string name, string address, string description, string daysAvailable, string openHours, int restTables)
        {
            //if (CheckExists(name) == false)
            //{
                //set member variables
                this.restaurantID = GetNewRestaurantID();
                this.name = name;
                this.address = address;
                this.description = description;
                this.daysAvailable = daysAvailable;
                this.openHours = openHours;
                this.restTables = restTables;

                //RegisterRestaurant(name, address, description, daysAvailable, openHours, image, restTables);

            //}

        }

        public Restaurant(int restaurantID, string name, string address, string description, string daysAvailable, string openHours, int restTables)
        {

            //set member variables
            this.restaurantID = restaurantID;
            this.name = name;
            this.address = address;
            this.description = description;
            this.daysAvailable = daysAvailable;
            this.openHours = openHours;
            this.restTables = restTables;

            

        }

        public int GetRestaurantID()
        {
            return restaurantID;
        }

        public string GetRestaurantName()
        {
            return name;
        }

        public string GetRestaurantDescription()
        {
            return description;
        }

        public string GetRestaurantAddress()
        {
            return address;
        }

        public int GetRestaurantTables()
        {
            return restTables;
        }

        public string GetRestaurantHours()
        {
            return openHours;
        }

        private int GetNewRestaurantID()
        {


            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);
            object scalarVal;
            int newRestID = 0;

            sqlCmd.CommandText = "select MAX(rest_ID) from tbl_Restaurants";
            sqlCmd.Connection = sqlConnection;

            sqlConnection.Open();
            scalarVal = sqlCmd.ExecuteScalar();

            if (scalarVal == null || scalarVal == System.DBNull.Value)
            {
                newRestID = 0;

            }
            else
            {
                newRestID = (int)sqlCmd.ExecuteScalar();


            }

            sqlConnection.Close();
            return newRestID + 1;

        }


        //get info for viewing on Manage Bookings screen
        public string GetRestaurantDisplay()
        {
            return name + "#" + address + "#" + description;
        }

        //get info for edit on Manage Restaurant screen
        public string GetRestaurantEdit()
        {
            return this.name + "#" + this.address + "#" + this.description + "#" + daysAvailable + "#" + openHours + "#" + restTables;
        }

        //recieves restaurant data and inserts into database
        public void RegisterRestaurant(string name, string address, string description, string daysAvailable, string openHours, int restTables, int auserid)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            //create Insert command
            sqlCmd.CommandType = System.Data.CommandType.Text;


            sqlCmd.CommandText = "INSERT INTO tbl_Restaurants (rest_name, rest_address, rest_description, rest_daysAvailable, rest_openHours, rest_tables, rest_owner) VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7)";
            sqlCmd.Parameters.AddWithValue("@param1", name);
            sqlCmd.Parameters.AddWithValue("@param2", address);
            sqlCmd.Parameters.AddWithValue("@param3", description);
            sqlCmd.Parameters.AddWithValue("@param4", daysAvailable);
            sqlCmd.Parameters.AddWithValue("@param5", openHours);
            sqlCmd.Parameters.AddWithValue("@param6", restTables);
            sqlCmd.Parameters.AddWithValue("@param7", auserid);


            sqlCmd.Connection = sqlConnection;


            sqlConnection.Open();
            sqlCmd.ExecuteNonQuery();

            sqlConnection.Close();

        }



        //check if a restaurant exists
        //return true/false
        public bool CheckExists(string name)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);
            bool doesExist = false;
      
            string theQueryString = "select rest_name from tbl_Restaurants where rest_name = '" + name + "'";

            using (sqlConnection)
            {


                SqlCommand sqlCommand =
                        new SqlCommand(theQueryString, sqlConnection);
                sqlConnection.Open();



                SqlDataReader sqlReader = sqlCommand.ExecuteReader();



                while (sqlReader.Read())
                {

                    doesExist = true;
                }


                sqlReader.Close();
            }

            sqlConnection.Close();


           

            return doesExist;
        }


        public void RemoveRestaurant(string name)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            //create Insert command
            sqlCmd.CommandType = System.Data.CommandType.Text;

            sqlCmd.CommandText = "DELETE FROM tbl_Restaurants WHERE rest_name = @param1";

            sqlCmd.Parameters.AddWithValue("@param1", name);
            sqlCmd.Connection = sqlConnection;


            sqlConnection.Open();
            sqlCmd.ExecuteNonQuery();

            sqlConnection.Close();
        }


        public void UpdateRestaurant(int id, string name, string address, string description, string daysAvailable, string openHours, int restTables)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            //create Insert command
            sqlCmd.CommandType = System.Data.CommandType.Text;
 
            sqlCmd.CommandText = "UPDATE tbl_Restaurants SET rest_name = '" + name + "', rest_address = '" + address + "', rest_description = '" + description + "', rest_daysAvailable = '" + daysAvailable + "', rest_openHours = '" + openHours + "', rest_tables = '" + restTables + "' WHERE rest_ID = '" + id + "'";


           

            sqlCmd.Connection = sqlConnection;


            sqlConnection.Open();
            sqlCmd.ExecuteNonQuery();

            sqlConnection.Close();
        }

        public List<Restaurant> GetAllRestaurants(int userid)
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);
            string filter = "";

           

            if (userid != 0)
            {
                filter = "Where rest_owner = " + userid;
            }

            List<Restaurant> rests = new List<Restaurant>();

            string theQueryString = "select * from tbl_Restaurants " + filter + " ORDER BY rest_name";

            System.Diagnostics.Debug.WriteLine(theQueryString);

            using (sqlConnection)
            {


                SqlCommand sqlCommand =
                        new SqlCommand(theQueryString, sqlConnection);
                sqlConnection.Open();



                SqlDataReader sqlReader = sqlCommand.ExecuteReader();



                while (sqlReader.Read())
                {
                    

                    rests.Add(new Restaurant(sqlReader.GetInt32(0), sqlReader.GetString(1), sqlReader.GetString(2), sqlReader.GetString(3), sqlReader.GetString(4), sqlReader.GetString(5), sqlReader.GetInt32(6)));

                }


                sqlReader.Close();
            }

            sqlConnection.Close();


            return rests;
        }
    }

    
}
