using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ITCPRemotingService.Methods.Login
{
    class LoginMethods
    {
        // Returns the result of the Attempted Login
        public delegate void LoginResultEventHandler(bool Result);
        public event LoginResultEventHandler LoginResultEvent;
        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        Users.Users User = new Users.Users();

        internal void Login(string emailAddress, string Password)
        {
            if (LogEvent != null) LogEvent(emailAddress + " Is Loging in");
            // Does the User Exist in the Database?
            using (var db = new DamoclesEntities())
            {
                AddClient(emailAddress);   // Add the User's Client for this logon
                var uid = User.GetUserID(emailAddress);
                var user = db.Users.First(u => u.UserId == uid);
                user.IsOnline = true;   // Set the User to Online

                int rows = db.SaveChanges();
                UpdateLoginHistory(emailAddress); // Add the User's Logon History

                if (rows == 1)
                {
                    LogEvent(emailAddress + " is Logged In");
                    LoginResultEvent(true);
                }
                else
                {
                    LogEvent(emailAddress + " Failed to Log In");
                    LoginResultEvent(false);
                }
            }
        }

        private int AddClient(string emailAddress)
        {
            if (LogEvent != null) LogEvent(emailAddress + " Is Being Added in");
            int rid = 0;
            using (var db = new DamoclesEntities())
            {
                var c = db.Clients;
                var nclient = new Client();
                nclient.RaptorClientID = emailAddress;
                nclient.UserId = User.GetUserID(emailAddress);
                c.Add(nclient);
                db.SaveChanges();
                rid = nclient.ClientsID;

                return rid;
            }
        }

        internal void UpdateLoginHistory(string emailAddress)
        {
            if (LogEvent != null) LogEvent("User's Logon History is being Created.");
            using (var db = new DamoclesEntities())
            {
                var loh = db.LogonHistories;
                var lohe = new LogonHistory();
                lohe.LoggedOnDate = DateTime.UtcNow;
                lohe.UserId = User.GetUserID(emailAddress);
                loh.Add(lohe);
                int rows = db.SaveChanges();
                if (rows < 1)
                {
                    LogEvent(emailAddress + " Failed to Add User's Logon History Record ");
                }
            }
        }
    }


}
