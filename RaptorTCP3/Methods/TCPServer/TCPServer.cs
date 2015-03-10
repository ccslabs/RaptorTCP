using NetComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RaptorTCP3.Methods.Utilities;

namespace RaptorTCP3.Methods.TCPServer
{
    class TCPServer
    {
        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;


        NetComm.Host tcpServer = new Host(9119);

        Utilities.Utilities Utils = new Utilities.Utilities();

        private void Broadcast(string Message)
        {
            LogEvent("Broadcasting: " + Message); ;
            tcpServer.Brodcast(Utils.GetBytes(Message));
        }

        private void Reply(string ID, string callingCommand, string Result)
        {
            LogEvent("Replying to " + ID);
            string cr = callingCommand + " " + Result;
            byte[] commandResult = Utils.GetBytes(cr);
            tcpServer.SendData(ID, commandResult);

        }

        private void Reply(string ID, string callingCommand, string Result, string returnedValue)
        {
            LogEvent("Replying to " + ID);
            string cr = callingCommand + " " + Result + " " + returnedValue;
            byte[] commandResult = Utils.GetBytes(cr);
            tcpServer.SendData(ID, commandResult);
        }
     

    }
}
