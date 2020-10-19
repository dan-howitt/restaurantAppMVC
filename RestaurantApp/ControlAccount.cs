using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp
{
    class ControlAccount
    {
        //CONTROLLER (MVC)

        private Account newAcct;

        public ControlAccount(string username, string password, string role)
        {

            newAcct = new Account(username, password, role);

        }


        public int GetUserID()
        {

            return newAcct.GetUserID();
        }

        public string GetRole()
        {
            return newAcct.GetRole();
        }

        public string GetName()
        {
            return newAcct.GetName();
        }

        public bool RegisterAccount(string username, string password, string role)
        {
            return newAcct.RegisterAccount(username, password, role);
        }

        public string CheckLogin(string username, string password)
        {
            return newAcct.CheckLogin(username, password);
        }
    }
}
