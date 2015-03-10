using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaptorTCP3.Methods.SystemURLS
{
    class sUrls
    {

        // Returns the result of the Attempted Login
        public delegate void UrlsCountResultEventHandler(long Result);
        public event UrlsCountResultEventHandler UrlsCountResultEvent;
        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        internal void URLSCount()
        {
            LogEvent("Counting URLS");

            using (var db = new DamoclesEntities())
            {
                var urls = db.URLS;
                long result = urls.LongCount();
                LogEvent("Number of URLS in Database: " + result.ToString("N0"));
                UrlsCountResultEvent(result);
            }
        }

    }
}
