using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCPRemotingService
{
    public interface ITCPRemotingService
    {
         bool Login(string EmailAddress, string Password); // Client Logging in
         bool Register(string EmailAddress, string Password); // Client Registering
         string[] TakeUrls(); // Client wants some URLS to process
         bool GiveUrls(string[] UrlList); // Client Sending us URls it has processed

    }
}
