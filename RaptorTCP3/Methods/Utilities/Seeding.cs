using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaptorTCP3.Methods.Utilities
{
    class Seeding
    {
        private Utilities Utils = new Utilities();

        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;
        // Notify Progress Changed
        public delegate void ProgressChangedEventHandler(int Progress);
        public event ProgressChangedEventHandler ProgressChangedEvent;
        // Notify New Progress Maximum
        public delegate void ProgressMaximumChangedEventHandler(int Progress);
        public event ProgressMaximumChangedEventHandler ProgressMaximumChangedEvent;

        internal void SeedUrls()
        {
            // Load URLS from file
            FileStream fs = null;
            StreamReader sr = null;
            ArrayList alUrls = new ArrayList();

            try
            {
                fs = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "bookmarks_3_6_15.html"), FileMode.Open, FileAccess.Read, FileShare.None);
                sr = new StreamReader(fs);
                
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Replace("\t", "").Trim();
                    if (line.StartsWith("http://") || line.StartsWith("https://"))
                    {
                        string[] parts = line.Split(' ');
                        string url = parts[0].Replace("\"", "").Trim();
                        if (!alUrls.Contains(url)) alUrls.Add(url);
                    }
                 
                }
              
                sr.Close();
                fs.Close();


                SaveUrls(alUrls);
            }
            catch (Exception ex)
            {
                LogEvent("Error Loading Seed Urls (seedUrlsToolStripMenuItem_Click) " + ex.Message);
                if (sr != null) sr.Close();
                if (fs != null) fs.Close();
            }
        }

        private void SaveUrls(ArrayList alUrls)
        {
            LogEvent("Seeding URLS");
            int rows = 0;

            ProgressMaximumChangedEvent(alUrls.Count);

            using (var db = new DamoclesEntities())
            {
                DeleteAllURLS(db);

                var urls = db.URLS;
                int idx = 0;
                foreach (string url in alUrls)
                {
                    idx++;
                    ProgressChangedEvent(idx);
                    string newurl = Utils.DecodeUrlString(url);

                    urls.Add(AddUrl(url));
                }
                db.SaveChanges();
            }

            ProgressChangedEvent(0);
         
            LogEvent("Seeded URLS Table with " + rows.ToString("N0") + " rows");
            alUrls.Clear();
        }
    }
}
