using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCPRemotingService.Registration
{
    class Registration
    {
        private Users.Users users = new Users.Users();



        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        internal bool RegistrationSuccessful(string ID, string commandParams)
        {
            if (LogEvent != null) LogEvent(ID + " Registration was Successful");
            string[] command = commandParams.Trim().ToLowerInvariant().Split(' ');
            if (command[0] == "register")
            {
                return Register(ID, command[1], command[2]);
            }
            return false;
        }

        private bool Register(string ClientID, string emailAddress, string Password)
        {
            if (LogEvent != null) LogEvent("Registering User");
            using (var db = new DamoclesEntities())
            {
                System.Data.Entity.DbSet<User> users = db.Users;

                var eu = Users.CreateUser(ClientID, emailAddress, Password);
                users.Add(eu);

                int rows = db.SaveChanges();
                if (rows < 1)
                {
                    if (LogEvent != null) LogEvent("Failed to Add user: " + emailAddress);
                    return false;
                }
                else
                {
                    Login.UpdateLoginHistory(emailAddress);
                    return true;
                }
            }
        }

    }
}
