using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaptorTCP3.Methods.Login
{
    class LoginMethods
    {
        private bool LoginSuccessful(string Cid, string commandParams)
        {
            string[] command = commandParams.Trim().ToLowerInvariant().Split(' ');
            if (command[0] == "login")
            {
                return Login(Cid, command[1], command[2]);
            }
            return false;
        }

        private bool Login(string Cid, string emailAddress, string Password)
        {
            // Does the User Exist in the Database?
            using (var db = new DamoclesEntities())
            {

                var uid = GetUserID(emailAddress);
                var user = db.Users.First(u => u.UserId == uid);
                user.IsOnline = true;   // Set the User to Online
                //user.CurrentClientID = Cid;
                int rows = db.SaveChanges();
                UpdateLoginHistory(emailAddress); // Add the User's Logon History
                AddClient(Cid, emailAddress);   // Add the User's Client for this logon
                if (rows == 1)
                {
                    frmMain.Log(emailAddress + " is Logged In");
                    return true;
                }
                else
                {
                   frmMain.Log(emailAddress + " Failed to Log In");
                    return false;
                }
            }
        }
    }
}
