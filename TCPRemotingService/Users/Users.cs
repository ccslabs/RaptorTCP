using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCPRemotingService.Users
{
    class Users
    {
        public ITCPRemotingService.Licenses.Licenses license = new Licenses.Licenses();

        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        public delegate void UserCountEventHandler(int NumberOfUsers);
        public event UserCountEventHandler UserCountEvent;

        internal ObservableCollection<string> allUsers = new ObservableCollection<string>();

        internal Users()
        {
            allUsers.CollectionChanged += allUsers_CollectionChanged;
        }

        void allUsers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if (LogEvent != null) LogEvent("New User Joined");
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:                    
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    if (LogEvent != null) LogEvent("User Removed");
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
          if(UserCountEvent != null)  UserCountEvent(allUsers.Count);
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

        internal User CreateUser(string ClientID, string emailAddress, string Password)
        {
            if (LogEvent != null) LogEvent("Creating New User");
            var eu = new User();
            eu.Username = emailAddress;
            eu.UserPasswordHash = Password;
            eu.RegisteredDate = DateTime.UtcNow;
            eu.CountryId = 3;
            eu.StateId = 2;
            eu.JurisidictionId = 4;
            eu.LanguagesId = 1;
            eu.LicenseNumber = license.GenerateTemporaryLicenseNumber(emailAddress);
            eu.emailAddress = emailAddress;
            //TODO: ADD TRACKING OF THE VARIOUS CLIENTS THE USER HAS
            //  eu.UserClientID =  AddClient(ClientID, emailAddress);
            return eu;
        }

        internal  void DeleteAllUsers(DamoclesEntities db)
        {
           if(LogEvent != null) LogEvent("Deleting All Users.");
            var all = from c in db.Users select c;
            db.Users.RemoveRange(all);
            db.SaveChanges();
        }

        internal void AddUserToAllUsers(string Cid)
        {
            if (allUsers.Contains("Cid"))
                LogEvent(Cid + " User Rejoined");
            else
            {
                allUsers.Add(Cid);
            }
        }
    }
}
