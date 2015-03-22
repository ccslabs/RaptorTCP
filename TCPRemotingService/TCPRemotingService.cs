using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPRemotingService
{
    public class TCPRemotingService : MarshalByRefObject, ITCPRemotingService.ITCPRemotingService
    {


        public bool Login(string EmailAddress, string Password)
        {
            // Log the User In
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
