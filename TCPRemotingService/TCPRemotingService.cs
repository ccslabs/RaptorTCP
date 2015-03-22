using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TCPRemotingService
{
    public class TCPRemotingService : MarshalByRefObject, ITCPRemotingService
    {

        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;
        public delegate void LoginResultEventHandler(bool result);
        public event LoginResultEventHandler LoginResultEvent;


        public bool Login(string EmailAddress, string Password)
        {
            using (var db = new DamoclesEntities())
            {
                AddClient(EmailAddress);   // Add the User's Client for this logon
                var uid = GetUserID(EmailAddress);
                var user = db.Users.First(u => u.UserId == uid);
                user.IsOnline = true;   // Set the User to Online

                int rows = db.SaveChanges();
                UpdateLoginHistory(EmailAddress); // Add the User's Logon History

                if (rows == 1)
                {
                    LogEvent(EmailAddress + " is Logged In");
                    LoginResultEvent(true);
                    return true;
                }
                else
                {
                    LogEvent(EmailAddress + " Failed to Log In");
                    LoginResultEvent(false);
                    return false;
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
                nclient.UserId = GetUserID(emailAddress);
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
                lohe.UserId = GetUserID(emailAddress);
                loh.Add(lohe);
                int rows = db.SaveChanges();
                if (rows < 1)
                {
                    LogEvent(emailAddress + " Failed to Add User's Logon History Record ");
                }
            }
        }

        internal int GetUserID(string emailAddress)
        {
            if (LogEvent != null) LogEvent("Getting User ID from Email");
            using (var db = new DamoclesEntities())
            {
                var user = db.Users.First(u => u.emailAddress == emailAddress);
                return user.UserId;
            }
        }

        internal string GetUserEmailAddressByID(string Cid)
        {
            if (LogEvent != null) LogEvent("Getting User Email from ID");
            using (var db = new DamoclesEntities())
            {
                var user = db.Users.First(u => u.CurrentClientID == Cid && u.IsOnline == true);
                return user.emailAddress;
            }
        }

       

        public bool Register(string EmailAddress, string Password)
        {
            throw new NotImplementedException();
        }

        public string[] TakeUrls()
        {
            throw new NotImplementedException();
        }

        public bool GiveUrls(string[] UrlList)
        {
            throw new NotImplementedException();
        }

        public bool Hello()
        {
            return true;
        }
    }

}
