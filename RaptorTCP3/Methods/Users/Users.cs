using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaptorTCP3.Methods.Users
{
    class Users
    {

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
