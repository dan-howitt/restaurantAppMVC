using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantApp
{
    //VIEW (MVC)
    public partial class Form1 : Form
    {
       
        private ControlAccount theCurrentUser;

        private int activeRestaurant;

        private List<Restaurant> theRestaurants;
        

        public Form1()
        {
            InitializeComponent();
        }

        //hide login page, display register page
        private void btnNewUser_Click(object sender, EventArgs e)
        {
            grpRegister.Visible = true;
            grpLogin.Visible = false;
        }

        //register user and then redirect to manage bookings screen
        private void btnRegister_Click(object sender, EventArgs e)
        {
            string role;

            //get the user role
            if (radCustomer.Checked == true)
            {
                role = "Customer";
            }
            else
            {
                role = "Owner";
            }

            //set current user
            theCurrentUser = new ControlAccount(txtRegUser.Text, txtRegPass.Text, role);

            //register the account - returns true if succesful
            if (theCurrentUser.RegisterAccount(txtRegUser.Text, txtRegPass.Text, role) == true)
            {
                grpRegister.Visible = false;
                SetBookingsView(role);
              

                
            }
            else
            {
                theCurrentUser = null;
                MessageBox.Show("Username already exists!");
            }



        }


        //sets up the Manage Bookings screen using role
        private void SetBookingsView(string role)
        {
            grpManageBookings.Visible = true;

            //get all restaurants
            GetAllRestaurants();

            //load first restaurant into the display
            if (theRestaurants.Count > 0)
            {
                LoadRestaurantDisplay(0);
               
            }

            //configure navigation buttons
            if (theRestaurants.Count <= 1)
            {
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
            }

            if (role == "Customer")
            {
                btnEdit.Visible = false;
                btnNew.Visible = false;
                btnBook.Visible = true;
            }
            else if (role == "Owner")
            {
                btnEdit.Visible = true;
                btnNew.Visible = true;
                btnBook.Visible = false;
            }

           

            
        }


      

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string check;
            //set current user - set role as customer for now
            theCurrentUser = new ControlAccount(txtLogUser.Text, txtLogPass.Text, "NEW");

            check = theCurrentUser.CheckLogin(txtLogUser.Text, txtLogPass.Text);

            if (check == "nonuser") 
            {
                MessageBox.Show("Username does not exist!");
            }
            else if (check == "nonpassword")
            {
                MessageBox.Show("Incorrect Password!");
            }
            else
            {
                
                grpLogin.Visible = false;
                SetBookingsView(check);
            }
        }



 
        private void GetAllRestaurants()
        {
            //set temp Restaurant
            ControlRestaurant tempRest = new ControlRestaurant("NEW", "NEW", "NEW", "NEW", "NEW", 0);


            //get all Restaurants - check role first
            if (theCurrentUser.GetRole() == "Customer")
            {
                theRestaurants = tempRest.GetAllRestaurants(0);
            }
            else 
            {
                theRestaurants = tempRest.GetAllRestaurants(theCurrentUser.GetUserID());
            }
            
            //reset activeRestaurant
            if (theRestaurants.Count > 0)
            {
                activeRestaurant = 0;
            }

            
        }

        //loads a restaurant into the gui on Manage Bookings screen - enables users to view the Restaurants
        private void LoadRestaurantDisplay(int index)
        {
            //check if any restaurants available
            if(theRestaurants.Count != 0)
            {
                //get single restaurant as initial display
               // String aRestaurant = restaurants[index].GetRestaurantDisplay();
                //string[] aRest = aRestaurant.Split('#');
                lblRestaurantName.Text = theRestaurants[index].GetRestaurantName();
                lblDescription.Text = theRestaurants[index].GetRestaurantDescription();
                lblAddress.Text = theRestaurants[index].GetRestaurantAddress();

                //set new activeRestaurant
                activeRestaurant = index;
            }

            //enable/disable next nav button
            if (theRestaurants.Count <= 1 || index == theRestaurants.Count -1)
            {
                btnNext.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;
            }

            //enable/disable previous nav button
            if (theRestaurants.Count <= 1 || index == 0)
            {
                btnPrevious.Enabled = false;
            }
            else
            {
                btnPrevious.Enabled = true;
            }


        }

        //loads a restaurant into the gui on Manage Restaurant screen - enables users to edit the current active Restaurant
        private void LoadRestaurantEdit()
        {
            String aRestaurant = theRestaurants[activeRestaurant].GetRestaurantEdit();
            string[] aRest = aRestaurant.Split('#');
            lblRestid.Text = (activeRestaurant + 1).ToString(); 
            txtResName.Text = aRest[0];
            txtAddress.Text = aRest[1];
            txtDescription.Text = aRest[2];

            string[] theDays = aRest[3].Split(',');
            chkMon.Checked = bool.Parse(theDays[0]);
            chkTue.Checked = bool.Parse(theDays[1]);
            chkWed.Checked = bool.Parse(theDays[2]);
            chkThu.Checked = bool.Parse(theDays[3]);
            chkFri.Checked = bool.Parse(theDays[4]);
            chkSat.Checked = bool.Parse(theDays[5]);
            chkSun.Checked = bool.Parse(theDays[6]);

            string[] theHours = aRest[4].Split(',');
            chkAm.Checked = bool.Parse(theHours[0]);
            chkPm.Checked = bool.Parse(theHours[1]);

            txtTables.Text = aRest[5];
        }

        //take user from Manage bookings screen to Manage Restaurant
        private void btnNew_Click(object sender, EventArgs e)
        {
            grpManageBookings.Visible = false;
            grpManageRestaurant.Visible = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            grpManageBookings.Visible = false;
            grpManageRestaurant.Visible = true;

            //load current displayed restaurant into the manage restaurant screen
            LoadRestaurantEdit();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            int id;
            bool isIDValid = Int32.TryParse(lblRestid.Text, out id);
          
            string name = txtResName.Text;
            string description = txtDescription.Text;
            string address = txtAddress.Text;
            int tables;
            bool isTableValid = Int32.TryParse(txtTables.Text, out tables);

            //get days
            string days = chkMon.Checked + "," + chkTue.Checked + "," + chkWed.Checked + "," + chkThu.Checked + "," + chkFri.Checked + "," + chkSat.Checked + "," + chkSun.Checked;

            //get hours
            string hours = chkAm.Checked + "," + chkPm.Checked;

            //check input valid
            if (name == "" || description == "" || address == "" || isTableValid == false || chkAm.Checked == false && chkPm.Checked == false || chkMon.Checked == false && chkTue.Checked == false && chkWed.Checked == false && chkThu.Checked == false && chkFri.Checked == false && chkSat.Checked == false && chkSun.Checked == false)
            {
                MessageBox.Show("Please ensure all input is completed!");
            }
            else
            {
                //check if this is a new restaurant
                if(isIDValid == false)
                {
                    SaveNewRestaurant(name, address, description, days, hours, tables, theCurrentUser.GetUserID());
                }
                else
                {
                    UpdateRestaurant(id, name, address, description, days, hours, tables);
                }

                lblRestid.Text = "";
                txtResName.Text = "";
                txtDescription.Text = "";
                txtAddress.Text = "";
                txtTables.Text = "";

                chkMon.Checked = false;
                chkTue.Checked = false;
                chkWed.Checked = false;
                chkThu.Checked = false;
                chkFri.Checked = false;
                chkSat.Checked = false;
                chkSun.Checked = false;

                chkAm.Checked = false;
                chkPm.Checked = false;
            }

            
        }

        //when a user clicks the save button from the Manage Restaurant screen
        //this saves a new restaurant 
        private void SaveNewRestaurant(string name, string address, string description, string days, string hours, int tables, int userid)
        {
            //add to local data storage
            theRestaurants.Add(new Restaurant(name, address, description, days, hours, tables));

            //check if restaurant already exists
            if (theRestaurants[theRestaurants.Count - 1].CheckExists(name))
            {
                //remove added restaurant
                theRestaurants.RemoveAt(theRestaurants.Count - 1);

                MessageBox.Show("Restaurant already registered!");
            }
            else
            {
                //add permenant storage
                theRestaurants[theRestaurants.Count - 1].RegisterRestaurant(name, address, description, days, hours, tables, userid);

                MessageBox.Show("Restaurant successfully registered!");

                //reload restaurants
                GetAllRestaurants();

                //return user to manage bookings screen
                grpManageRestaurant.Visible = false;
                grpManageBookings.Visible = true;

                //load first restaurant into the display
                LoadRestaurantDisplay(0);


            }
        }

        //when a user clicks the save button from the Manage Restaurant screen
        //this updates an existing restaurant 
        private void UpdateRestaurant(int id, string name, string address, string description, string days, string hours, int tables)
        {
            

            //add permenant storage
            theRestaurants[0].UpdateRestaurant(id, name, address, description, days, hours, tables);

            MessageBox.Show("Restaurant successfully updated!");

            //reload restaurants
            GetAllRestaurants();

            //return user to manage bookings screen
            grpManageRestaurant.Visible = false;
            grpManageBookings.Visible = true;

            //load first restaurant into the display
            LoadRestaurantDisplay(0);
            
        }

        //navigation buttons to move between restaurants
        private void btnPrevious_Click(object sender, EventArgs e)
        {           
            LoadRestaurantDisplay(activeRestaurant - 1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            LoadRestaurantDisplay(activeRestaurant + 1);
        }

        //hide register screen and return to login
        private void btnCloseRegister_Click(object sender, EventArgs e)
        {
            grpRegister.Visible = false;
            grpLogin.Visible = true;
        }


        //displays current bookings for current restaurant
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            int userID = 0;

            //set temp booking
            ControlBooking tempBook = new ControlBooking(0, DateTime.Today, 0, 0);

            //check role for display filter
            if(theCurrentUser.GetRole() == "Customer")
            {
                userID = theCurrentUser.GetUserID();
            }

            //get existing bookings
            List<string> bookings = tempBook.GetBookings(theRestaurants[activeRestaurant].GetRestaurantID(), userID);

            string[] splitBook;

            foreach (string book in bookings)
            {
                splitBook = book.Split(',');

                string[] row = { splitBook[0], splitBook[1], splitBook[2], splitBook[3], splitBook[4] };
                var listViewItem = new ListViewItem(row);
                lstBookings.Items.Add(listViewItem);

            }

            
        }

        //allows the user to book a table at the selected restaurant
        private void btnBook_Click(object sender, EventArgs e)
        {
            //set restaurant name and customer name into the grpBookings screen
            lblBookRestName.Text = lblRestaurantName.Text;
            lblBookCustName.Text = theCurrentUser.GetName();

            grpManageBookings.Visible = false;
            grpBookings.Visible = true;

            //set up available times
            GetAvailableTimes(DateTime.Today.ToString());
        }

        private void GetAvailableTimes(string aDate)
        {
            List<int> availableTimes = new List<int>();

            //set temp booking
            ControlBooking tempBook = new ControlBooking(0, DateTime.Today, 0, 0);

            //get existing bookings
            List<int> bookedTimes = tempBook.GetTimes(theRestaurants[activeRestaurant].GetRestaurantID(), aDate, theRestaurants[activeRestaurant].GetRestaurantTables());


            //get openingTimes
            string openingTimes = theRestaurants[activeRestaurant].GetRestaurantHours();

            string[] splitOpenTimes = openingTimes.Split(',');

            //build availableTimes
            if (splitOpenTimes[0] == "True")
            {
                availableTimes.Add(10);
                availableTimes.Add(11);
                availableTimes.Add(12);

            }

            if (splitOpenTimes[1] == "True")
            {
                availableTimes.Add(18);
                availableTimes.Add(19);
                availableTimes.Add(20);

            }



            //check and remove bookedTimes from availableTimes
            if (bookedTimes.Count > 0)
            {
                foreach (int time in bookedTimes)
                {
                    availableTimes.Remove(time);
                }


            }

            //output availableTimes
            lstBookTimes.DataSource = availableTimes;
        }

        private void btnDateOk_Click(object sender, EventArgs e)
        {

            GetAvailableTimes(dteBookDate.Text);
           
        }

        //register/update a booking
        private void btnSaveBooking_Click(object sender, EventArgs e)
        {
            //set temp Restaurant
            ControlBooking tempBook = new ControlBooking(0, DateTime.Today, 0, 0);

            int id;
            bool isIDValid = Int32.TryParse(lblBookingID.Text, out id);

            if(isIDValid)
            {
                //update booking
            }
            else
            {
                //register new booking
                tempBook.BookTable(theRestaurants[activeRestaurant].GetRestaurantID(), dteBookDate.Text, theCurrentUser.GetUserID(), (int)lstBookTimes.SelectedItem);
                MessageBox.Show("Table successfully booked!");
            
            }

            grpBookings.Visible = false;
            grpManageBookings.Visible = true;
            LoadRestaurantDisplay(activeRestaurant);
        }

        //allows bookings to be edited
        private void lstBookings_Click(object sender, EventArgs e)
        {
            string[] tempDate;
            string bookDate;

            if (lstBookings.SelectedItems.Count > 0)
            {

                ListViewItem item = lstBookings.SelectedItems[0];

                //change date format from 26/10/2020 00:00:00 to 2020-10-26
                tempDate = item.SubItems[2].Text.Split(' ');
                tempDate = tempDate[0].Split('/');
                bookDate = tempDate[2] + "-" + tempDate[1] + "-" + tempDate[0];

                //set id, restaurant/customer name and date into the grpBookings screen
                lblBookRestName.Text = item.SubItems[0].Text;
                lblBookCustName.Text = item.SubItems[1].Text;
                dteBookDate.Value = DateTime.Parse(bookDate);

               

                grpManageBookings.Visible = false;
                grpBookings.Visible = true;

                //set up available times
                GetAvailableTimes(bookDate);
            }
        }
    }
}
