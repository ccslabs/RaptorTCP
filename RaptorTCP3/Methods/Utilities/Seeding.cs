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
        private void SeedUrls()
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
                Utils.Log("Error Loading Seed Urls (seedUrlsToolStripMenuItem_Click) " + ex.Message);
                if (sr != null) sr.Close();
                if (fs != null) fs.Close();
            }
        }

    }
}
