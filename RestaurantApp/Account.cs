using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Transactions;

namespace RestaurantApp
{
    //MODEL (MVC)

    class Account
    {
        //member variables
        private int userID;
        private string username;
        private string password;
        private string role;
       
        

        //constructor
        //create new account
        public Account(string username, string password, string role)
        {
           

            //set member variables
            this.userID = GetNewUserID();
            this.username = username;
            this.password = password;
            this.role = role;

            //RegisterAccount(username, password, role);

           
        }


        private int GetNewUserID()
        {
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);
            object scalarVal;
            int newID;

            sqlCmd.CommandText = "select MAX(acct_ID) from tbl_Accounts;";
            sqlCmd.Connection = sqlConnection;

            sqlConnection.Open();
            
            scalarVal = sqlCmd.ExecuteScalar();

            if (scalarVal == null || scalarVal == System.DBNull.Value)
            {
                newID = 0;
               
            }
            else
            {
                newID = (int)sqlCmd.ExecuteScalar();

               
            }
               

            sqlConnection.Close();
            return newID + 1;

        }

        public int GetUserID()
        {
            return this.userID;
        }

        public string GetRole()
        {
            return this.role;
        }

        public string GetName()
        {
            return this.username;
        }

        //inserts new account into Database
        public bool RegisterAccount(string username, string password, string role)
        {
            bool isSuccess = true;
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);

            //check if the user doesnt already exist
            if (CheckLogin(username, password) == "nonuser")
            {
                //create Insert command
                sqlCmd.CommandType = System.Data.CommandType.Text;

                //set command details
                sqlCmd.CommandText = "INSERT INTO tbl_Accounts (acct_username, acct_password, acct_role) VALUES (@param1, @param2, @param3)";
                sqlCmd.Parameters.AddWithValue("@param1", username);
                sqlCmd.Parameters.AddWithValue("@param2", password);
                sqlCmd.Parameters.AddWithValue("@param3", role);

                

                //open sql and run ins
                sqlConnection.Open();
                sqlCmd.Connection = sqlConnection;
                sqlCmd.ExecuteNonQuery();

                sqlConnection.Close();
            }
            else
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        //check user credentials in database
        //return user role if correct credentials, nonuser if user doesnt exist, nonpassword if password is incorrect
        public string CheckLogin(string username, string password)
        {
            int userid = 0;
            string status = "nonuser";
            string sqlPassword = "";
            string theQueryString = "select acct_ID, acct_role, acct_password from tbl_accounts where acct_username = '" + username + "'";
            SqlCommand sqlCmd = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnectionString);

           

            using (sqlConnection)
            {


                SqlCommand sqlCommand =
                        new SqlCommand(theQueryString, sqlConnection);
                sqlConnection.Open();



                SqlDataReader sqlReader = sqlCommand.ExecuteReader();



                while (sqlReader.Read())
                {
                    userid = sqlReader.GetInt32(0);
                    status = sqlReader.GetString(1);
                    sqlPassword = sqlReader.GetString(2);
                }


                sqlReader.Close();
            }

            sqlConnection.Close();

           

            if (status != "nonuser")
            {
                if (sqlPassword != password)
                {
                    status = "nonpassword";
                }
                else
                {
                    this.userID = userid;
                    this.role = status;
                }
            }


            return status;
        }

       


    }
}
