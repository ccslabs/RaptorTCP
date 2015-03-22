using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TCPRemotingService
{
    public class TCPRemotingService : MarshalByRefObject, ITCPRemotingService.ITCPRemotingService
    {
        ITCPRemotingService.Methods.Login.LoginMethods login = new ITCPRemotingService.Methods.Login.LoginMethods();

        public bool Login(string EmailAddress, string Password)
        {
            login.LogEvent += login_LogEvent;
            login.LoginResultEvent += login_LoginResultEvent;

        }

        void login_LoginResultEvent(bool Result)
        {
            throw new NotImplementedException();
        }

        void login_LogEvent(string Message)
        {
            throw new NotImplementedException();
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
