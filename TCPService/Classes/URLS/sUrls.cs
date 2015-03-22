using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TCPService.Classes;

namespace TCPService.Classes.URLS.SystemURLS
{
    class sUrls
    {
        private Utilities.Utilities Utils = new Utilities.Utilities();
        private System.Timers.Timer timerMonitor = new System.Timers.Timer();

        internal Queue<string> urlQueue = new Queue<string>();

        // Returns the result Number of URLS Counted
        public delegate void UrlsCountResultEventHandler(long Result);
        public event UrlsCountResultEventHandler UrlsCountResultEvent;

        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        // Informs the main program that there are no more URLS to process
        public delegate void NoUrlsLeftToProcessEventHandler();
        public event NoUrlsLeftToProcessEventHandler NoUrlsLeftToProcessEvent;

        // Informs the main program that there are more URLS to process
        public delegate void MoreUrlsLeftToProcessEventHandler();
        public event MoreUrlsLeftToProcessEventHandler MoreUrlsLeftToProcessEvent;

        // Notify Progress Changed
        public delegate void ProgressChangedEventHandler(int Progress);
        public event ProgressChangedEventHandler ProgressChangedEvent;
        // Notify New Progress Maximum
        public delegate void ProgressMaximumChangedEventHandler(int Progress);
        public event ProgressMaximumChangedEventHandler ProgressMaximumChangedEvent;


        public sUrls()
        {
            timerMonitor.AutoReset = true;
            timerMonitor.Interval = 30000; // Check each 30 Seconds
            timerMonitor.Elapsed += timerMonitor_Elapsed;
            timerMonitor.Stop(); // Make sure it is not running.

        }

        void timerMonitor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (UrlsToProcessCount() > 0)
            {
                if (MoreUrlsLeftToProcessEvent != null) MoreUrlsLeftToProcessEvent();
                timerMonitor.Stop();
            }
        }

        internal void URLSCount()
        {
            if (LogEvent != null) LogEvent("Counting URLS");

            using (var db = new DamoclesEntities())
            {
                var urls = db.URLS;
                long result = urls.LongCount();
                if (LogEvent != null) LogEvent("Number of URLS in Database: " + result.ToString("N0"));
                UrlsCountResultEvent(result);
            }
        }

        internal long UrlsToProcessCount()
        {
            if (LogEvent != null) LogEvent("Counting URLS that still need to be processed");
            long result = -1;
            using (var db = new DamoclesEntities())
            {
                var urls = db.URLS;
                result = urls.Where(u => u.IsInProcessingQueue == false).LongCount();
                if (result > 0)
                {
                    if (MoreUrlsLeftToProcessEvent != null) MoreUrlsLeftToProcessEvent();
                    if (LogEvent != null) LogEvent("Number of URLS To Process in Database: " + result.ToString("N0"));
                    StopURLMonitor();
                }
                else
                {
                    if (LogEvent != null) LogEvent("NO URLS Left to be processed");
                    if (NoUrlsLeftToProcessEvent != null) NoUrlsLeftToProcessEvent();
                    StartURLMonitor();
                }

                return result;
            }
        }

        internal void PopulateURLQueue(long numberOfUrlsToGet)
        {
            StopURLMonitor();
            long toprocess = UrlsToProcessCount();
            if (toprocess > 0)
            {
                if (toprocess < numberOfUrlsToGet) numberOfUrlsToGet = toprocess;
                if (LogEvent != null) LogEvent("Populating URL Queue, Adding " + numberOfUrlsToGet);
                if (ProgressMaximumChangedEvent != null) ProgressMaximumChangedEvent(int.Parse(numberOfUrlsToGet.ToString()));

                using (var db = new DamoclesEntities())
                {
                    var urls = db.URLS;
                    for (int idx = 0; idx < numberOfUrlsToGet; idx++)
                    {
                        var result = urls.FirstOrDefault(u => u.IsInProcessingQueue == false);
                        var urlp = result.URLPath;
                        result.JoinedProcessingQueueDate = DateTime.UtcNow;
                        urlQueue.Enqueue(urlp);
                        if (ProgressMaximumChangedEvent != null && ProgressChangedEvent != null) ProgressChangedEvent(idx);
                        SetUrlToInProcessingQueue(urlp);
                        db.SaveChanges();
                    }
                }
                if (ProgressMaximumChangedEvent != null && ProgressChangedEvent != null) ProgressChangedEvent(0);
                if (LogEvent != null) LogEvent("Populating URL Queue Completed ");
                if (MoreUrlsLeftToProcessEvent != null) MoreUrlsLeftToProcessEvent(); // Tell the main program it can start sending URLS again
            }
            else
            {
                // This is a FATAL Problem, It may require 
                // Manually Adding more URLS
                // Returning URLS that have been in the processing queue for a shorter than normal period of time to be returned to the processing queue
                // This event should not be fired in normal operations but can occur during debugging.
                if (LogEvent != null) LogEvent("Failed to Populate URL Queue - there are no URLS left to Process");
                if (NoUrlsLeftToProcessEvent != null) NoUrlsLeftToProcessEvent();
                StartURLMonitor();
            }
        }

        private void StopURLMonitor()
        {
            if (LogEvent != null) LogEvent("Stopping URL Monitor");
            timerMonitor.Stop();
        }

        private void StartURLMonitor()
        {
            if (LogEvent != null) LogEvent("Starting URL Monitor");
            timerMonitor.Start();
        }

        private void SetUrlToInProcessingQueue(string url)
        {

            UpdateUrlInQueueStatus(url);
        }

        internal void UpdateUrlInQueueStatus(string url)
        {


            if (!string.IsNullOrEmpty(url))
            {
                using (var db = new DamoclesEntities())
                {
                    var urls = db.URLS;
                    var result = urls.FirstOrDefault(u => u.URLPath == url);
                    result.IsInProcessingQueue = true;
                    int rows = db.SaveChanges();
                    if (rows < 1)
                        LogEvent("Failed to Set Url to IsInProcessingQueue = True " + url);

                }
            }
        }

        internal void DeleteAllURLS(DamoclesEntities db)
        {
            if (LogEvent != null) LogEvent("Deleting All URLS");
            var all = from c in db.URLS select c;
            db.URLS.RemoveRange(all);
            db.SaveChanges();
        }

        internal URL AddUrl(string url)
        {
            if (LogEvent != null) LogEvent("Adding New URL");
            URL ue = new URL();
            ue.UrlHash = Utils.HashPassword(url);
            ue.URLPath = url;
            ue.DiscoveredById = 1006;
            ue.DiscoveryDate = DateTime.UtcNow;
            ue.IsInProcessingQueue = false;
            return ue;
        }

        internal void SaveUrl(string URL)
        {
            using (var db = new DamoclesEntities())
            {
                var urls = db.URLS;
                string newurl = Utils.DecodeUrlString(URL);

                //TODO: ADD NEW EVENT ADD URL
                AddUrl(URL);
                int row = db.SaveChanges();
                if (row > 1)
                {
                    if (LogEvent != null) LogEvent("Saved URL To Database " + URL);
                }
                else
                {
                    if (LogEvent != null) LogEvent("Failed to Save URL To Database " + URL);
                }
            }
        }
    }
}
