using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaptorTCP3.Methods.SystemURLS
{
    class sUrls
    {

        // Returns the result Number of URLS Counted
        public delegate void UrlsCountResultEventHandler(long Result);
        public event UrlsCountResultEventHandler UrlsCountResultEvent;
        // Returns the URL to Enqueue
        public delegate void UrlsToEnqueueEventHandler(string URL);
        public event UrlsToEnqueueEventHandler UrlsToEnqueueEvent;
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

        private void PopulateURLQueue(int numberOfUrlsToGet)
        {
            LogEvent("Populating URL Queue, Adding " + numberOfUrlsToGet);

            using (var db = new DamoclesEntities())
            {
                var urls = db.URLS;
                for (int idx = 0; idx < numberOfUrlsToGet; idx++)
                {
                    var result = urls.FirstOrDefault(u => u.IsInProcessingQueue == false);
                    var urlp = result.URLPath;
                    result.JoinedProcessingQueueDate = DateTime.UtcNow;
                    UrlsToEnqueueEvent(urlp);
                    SetUrlToInProcessingQueue(urlp);                    
                    db.SaveChanges();
                }
            }
           

        }

        private void SetUrlToInProcessingQueue(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                using (var db = new DamoclesEntities())
                {
                    var urls = db.URLS;
                    var result = urls.FirstOrDefault(u => u.URLPath == url);
                    result.IsInProcessingQueue = true;
                    int rows = db.SaveChanges();
                    if (rows < 1) LogEvent("Failed to Set Url to IsInProcessingQueue = True " + url);
                }
            }
        }

        private void UpdateUrlInQueueStatus(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                using (var db = new DamoclesEntities())
                {
                    var urls = db.URLS;
                    var result = urls.FirstOrDefault(u => u.URLPath == url);
                    result.IsInProcessingQueue = true;
                    int rows = db.SaveChanges();
                    if (rows < 1) LogEvent("Failed to Set Url to IsInProcessingQueue = True " + url);
                }
            }
        }
    }
}
