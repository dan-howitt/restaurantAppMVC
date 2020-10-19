using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp
{
    class ControlRestaurant
    {
        private Restaurant newRestaurant;

        public ControlRestaurant(string name, string address, string description, string daysAvailable, string openHours, int restTables)
        {
            newRestaurant = new Restaurant(name, address, description, daysAvailable, openHours, restTables);
        }

        public List<Restaurant> GetAllRestaurants(int userid)
        {
            return newRestaurant.GetAllRestaurants(userid);
        }

        public int GetRestaurantID()
        {
            return newRestaurant.GetRestaurantID();
        }

        public string GetRestaurantName()
        {
            return newRestaurant.GetRestaurantName();
        }

        public string GetRestaurantDescription()
        {
            return newRestaurant.GetRestaurantDescription();
        }

        public string GetRestaurantAddress()
        {
            return newRestaurant.GetRestaurantAddress();
        }

        public int GetRestaurantTables()
        {
            return newRestaurant.GetRestaurantTables();
        }

        public string GetRestaurantHours()
        {
            return newRestaurant.GetRestaurantHours();
        }

        public string GetRestaurantEdit()
        {
            return newRestaurant.GetRestaurantEdit();
        }

        public bool CheckExists(string name)
        {
            return newRestaurant.CheckExists(name);
        }

        public void RegisterRestaurant(string name, string address, string description, string days, string hours, int tables, int userid)
        {
            newRestaurant.RegisterRestaurant(name, address, description, days, hours, tables, userid);
        }

        public void UpdateRestaurant(int id, string name, string address, string description, string days, string hours, int tables)
        {
            newRestaurant.UpdateRestaurant(id, name, address, description, days, hours, tables);
        }
    }
}
