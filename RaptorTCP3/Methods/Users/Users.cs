using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaptorTCP3.Methods.Users
{
    class Users
    {
        public RaptorTCP3.Methods.Licenses.Licenses license = new Licenses.Licenses();

        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        public delegate void UserCountEventHandler(int NumberOfUsers);
        public event UserCountEventHandler UserCountEvent;

        internal ObservableCollection<string> allUsers = new ObservableCollection<string>();

        public Users()
        {
            allUsers.CollectionChanged += allUsers_CollectionChanged;
        }

        void allUsers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UserCountEvent(allUsers.Count);
        }

        internal int GetUserID(string emailAddress)
        {
            using (var db = new DamoclesEntities())
            {
                var user = db.Users.First(u => u.emailAddress == emailAddress);
                return user.UserId;
            }
        }

        internal string GetUserEmailAddressByID(string Cid)
        {
            using (var db = new DamoclesEntities())
            {
                var user = db.Users.First(u => u.CurrentClientID == Cid && u.IsOnline == true);
                return user.emailAddress;
            }
        }

        internal User CreateUser(string ClientID, string emailAddress, string Password)
        {
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

        internal static void DeleteAllUsers(DamoclesEntities db)
        {
            var all = from c in db.Users select c;
            db.Users.RemoveRange(all);
            db.SaveChanges();
        }
    }
}
