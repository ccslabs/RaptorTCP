using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RaptorTCP3.Methods.Utilities;

namespace RaptorTCP3.Methods.SqlClient
{
    class SqlClient
    {
        SystemURLS.sUrls sURLS = new SystemURLS.sUrls();
        Seeding Seeding = new Seeding();

        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        public SqlClient()
        {
           if(LogEvent != null)LogEvent("Starting SQL Client");

            if (sURLS.urlQueue.Count() < 50)
            {
               if(LogEvent != null)LogEvent("Populating URL QUEUE");
                sURLS.PopulateURLQueue(50);
            }
        }

    }
}
