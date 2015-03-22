using System;
using System.Collections;
using System.IO;


namespace RaptorTCP3.Methods.Utilities
{
    class Seeding
    {
        private Utilities Utils = new Utilities();


        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;
       

        public delegate void BroadcastWaitEventHandler();
        public event BroadcastWaitEventHandler BroadcastWaitEvent;
        public delegate void BroadcastResumeEventHandler();
        public event BroadcastResumeEventHandler BroadcastResumeEvent;

        public delegate void DeleteAllUrlsEventHandler(DamoclesEntities db);
        public event DeleteAllUrlsEventHandler DeleteAllUrlsEvent;
        public delegate void AddUrlEventHandler(string urlPath);
        public event AddUrlEventHandler AddUrlEvent;


        public Seeding()
        {

        }

        internal void SeedUrls()
        {
            if (BroadcastWaitEvent != null) BroadcastWaitEvent();
            if (LogEvent != null) LogEvent("Loading URL Seed Data");
            // Load URLS from file
            FileStream fs = null;
            StreamReader sr = null;
            ArrayList alUrls = new ArrayList();

            try
            {
                fs = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "bookmarks_3_6_15.html"), FileMode.Open, FileAccess.Read, FileShare.None);
                sr = new StreamReader(fs);

                //TODO: Add Progress Update here
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
                BroadcastResumeEvent();
            }
            catch (Exception ex)
            {
                if (LogEvent != null) LogEvent("FATAL Error Loading Seed Urls (seedUrlsToolStripMenuItem_Click) " + ex.Message);
                if (sr != null) sr.Close();
                if (fs != null) fs.Close();
            }
        }

        private void SaveUrls(ArrayList alUrls)
        {
            using (var db = new DamoclesEntities())
            {
                DeleteAllUrlsEvent(db);
            }
            foreach (string url in alUrls)
            {

                string newurl = Utils.DecodeUrlString(url);

                AddUrlEvent(newurl);
            }


        }

        internal void SeedUsers()
        {
            if (LogEvent != null) LogEvent("Seeding Users");
            using (var db = new DamoclesEntities())
            {
                //  Users.DeleteAllUsers(db);

                var usrs = db.Users;
                var su = new User();
                var em = "dave@ccs-labs.com";
                SeedAdminUser(db, usrs, su, em);

                su = new User();
                em = "system@ccs-labs.com";
                SeedSystemUser(db, usrs, su, em);

            }
        }

        private void SeedSystemUser(DamoclesEntities db, System.Data.Entity.DbSet<User> usrs, User su, string em)
        {
            if (LogEvent != null) LogEvent("Seeding System User");
            su.emailAddress = em;
            su.UserPasswordHash = "097dfd905dfa0e078883b7afcf7e653dde569bb1ed2ce3384d9c9ed7b85741d6e8d1b1a356318805d3c8b31b36a9916936d005d8134fb015d0392cf75cd7fa24";
            su.RegisteredDate = DateTime.UtcNow;
            su.CountryId = 3;
            su.StateId = 2;
            su.JurisidictionId = 4;
            su.LanguagesId = 1;
            su.IsOnline = false;
            su.AccountStatusId = 3;
            //  su.LicenseNumber = license.GenerateTemporaryLicenseNumber(em);
            su.emailAddress = em;
            //su.UserClientID = null;
            //su.CurrentClientID = null;
            usrs.Add(su);
            db.SaveChanges();
        }

        private void SeedAdminUser(DamoclesEntities db, System.Data.Entity.DbSet<User> usrs, User su, string em)
        {
            if (LogEvent != null) LogEvent("Seeding Admin User");
            su.emailAddress = em;
            su.UserPasswordHash = "097dfd905dfa0e078883b7afcf7e653dde569bb1ed2ce3384d9c9ed7b85741d6e8d1b1a356318805d3c8b31b36a9916936d005d8134fb015d0392cf75cd7fa24";
            su.RegisteredDate = DateTime.UtcNow;
            su.CountryId = 3;
            su.StateId = 2;
            su.JurisidictionId = 4;
            su.LanguagesId = 1;
            su.IsOnline = false;
            su.AccountStatusId = 3;
            //   su.LicenseNumber = license.GenerateTemporaryLicenseNumber(em);
            su.emailAddress = em;
            su.UserClientID = null;
            su.CurrentClientID = null;
            usrs.Add(su);
            db.SaveChanges();
        }
    }
}
