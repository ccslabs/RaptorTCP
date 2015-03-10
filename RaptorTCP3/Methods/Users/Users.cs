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
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        public delegate void UserCountEventHandler(int NumberOfUsers);
        public event UserCountEventHandler UserCountEvent;

        private ObservableCollection<string> allUsers = new ObservableCollection<string>();

        public void Users()
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
    }
}
