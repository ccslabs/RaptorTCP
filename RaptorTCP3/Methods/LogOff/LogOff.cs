using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaptorTCP3.Methods.LogOff
{
    class LogOff
    {

        private Users.Users Users = new Users.Users();
        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        internal void LogOffUser(string Cid)
        {
            //TODO: We need to store the User Clients IDs when they Utils.Log on so we can Utils.Log them off properly at the end!
            if (Cid.Contains("-")) // Internally generated IDs do not contains hyphens.
            {

                using (var db = new DamoclesEntities())
                {
                    var user = db.Users.First(u => u.CurrentClientID  == Cid);
                    user.IsOnline = false;
                    int rows = db.SaveChanges();
                    string emailAddress = Users.GetUserEmailAddressByID(Cid);
                    UpdateLogOffHistory(emailAddress);
                    if (rows == 1)
                    {
                        LogEvent(emailAddress + " is Logged Out");

                    }
                    else
                    {
                        LogEvent(emailAddress + " Failed to Log Out");

                    }
                }
            }
        }

        private void UpdateLogOffHistory(string emailAddress)
        {
            using (var db = new DamoclesEntities())
            {
                var uid = Users.GetUserID(emailAddress);
                var loh = db.LogonHistories.First(lh => lh.LoggedOffDate == null && lh.UserId == uid);
                loh.LoggedOffDate = DateTime.UtcNow;
                db.SaveChanges();

                var user = db.Users.First(u => u.UserId == uid);
                user.IsOnline = false;
                user.CurrentClientID = null;
                db.SaveChanges();
            }
        }

        internal void LogOffAllUsers()
        {

            foreach (string Cid in Users.allUsers)
            {
                LogOffUser(Cid);
            }

        }
    }
}
