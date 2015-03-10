using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaptorTCP3.Methods.SqlClient
{
    class SqlClient
    {

        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        internal void StartSQLClient()
        {


            LogEvent("Starting SQL Client");
            con.Open();
            if (URLSCount() == 0)
            {
                SeedUrls();
            }

            if (urlQueue.Count() < 50)
            {
                PopulateURLQueue(50);
            }
        }

        private void SubscribeToSQLEvents()
        {
            LogEvent("Subscribing to SQL Events");
            con.Disposed += con_Disposed;
            con.StateChange += con_StateChange;
            con.InfoMessage += con_InfoMessage;
            con.FireInfoMessageEventOnUserErrors = true;

        }

    }
}
