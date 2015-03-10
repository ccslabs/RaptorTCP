using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaptorTCP3.Methods.SystemURLS
{
    class sUrls
    {
        private RaptorTCP3.Methods.Utilities.Utilities Utils = new Utilities.Utilities();

        internal Queue<string> urlQueue = new Queue<string>();

        // Returns the result Number of URLS Counted
        public delegate void UrlsCountResultEventHandler(long Result);
        public event UrlsCountResultEventHandler UrlsCountResultEvent;
        // Returns the URL to Enqueue
        public delegate void UrlsToEnqueueEventHandler(string URL);
        public event UrlsToEnqueueEventHandler UrlsToEnqueueEvent;
        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        public sUrls()
        {
            if (urlQueue.Count() < 50)
            {                
                PopulateURLQueue(50);
            }
        }
       

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

        internal void PopulateURLQueue(int numberOfUrlsToGet)
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
                    urlQueue.Enqueue(urlp);
                    SetUrlToInProcessingQueue(urlp);                    
                    db.SaveChanges();
                }
            }
           

        }

        private void SetUrlToInProcessingQueue(string url)
        {
            UpdateUrlInQueueStatus(url);
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
                    else
                        LogEvent("URL Status Now InProcessingQueue ");
                }
            }
        }

        private  void DeleteAllURLS(DamoclesEntities db)
        {
            LogEvent("Deleting All URLS");
            var all = from c in db.URLS select c;
            db.URLS.RemoveRange(all);
            db.SaveChanges();
        }

        private URL AddUrl(string url)
        {
            LogEvent("Adding New URL");
            URL ue = new URL();
            ue.UrlHash = Utils.HashPassword(url);
            ue.URLPath = url;
            ue.DiscoveredById = 1006;
            ue.DiscoveryDate = DateTime.UtcNow;
            ue.IsInProcessingQueue = false;
            return ue;
        }
    }
}
