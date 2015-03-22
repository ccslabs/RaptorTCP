using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TCPService.Classes.Users;

namespace TCPService.Classes.LogOff
{
   public class LogOff
    {

       private Users.Users User = new Users.Users();
        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        public void LogOffUser(string Cid)
        {
            if (LogEvent != null) LogEvent(Cid + " Is Being Logged Off");
            //TODO: We need to store the User Clients IDs when they Utils.Log on so we can Utils.Log them off properly at the end!
            if (Cid.Contains("-")) // Internally generated IDs do not contains hyphens.
            {

                using (var db = new DamoclesEntities())
                {
                    var user = db.Users.First(u => u.CurrentClientID  == Cid);
                    user.IsOnline = false;
                    int rows = db.SaveChanges();
                    string emailAddress = User.GetUserEmailAddressByID(Cid);
                    
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
            if (LogEvent != null) LogEvent("Updating Users LogOff History");
            using (var db = new DamoclesEntities())
            {
                var uid = User.GetUserID(emailAddress);
                var loh = db.LogonHistories.First(lh => lh.LoggedOffDate == null && lh.UserId == uid);
                loh.LoggedOffDate = DateTime.UtcNow;
                db.SaveChanges();

                
                var us = db.Users.First(u => u.UserId == uid);
                us.IsOnline = false;
                us.CurrentClientID = null;
                db.SaveChanges();
            }
        }

        internal void LogOffAllUsers()
        {
            if (LogEvent != null) LogEvent("Logging Off ALL Users");
            foreach (string Cid in User.allUsers)
            {
                LogOffUser(Cid);
            }

        }
    }
}
