using NetComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RaptorTCP3.Methods.Utilities;
using System.Windows.Forms;
using RaptorTCP3.Methods.Users;

namespace RaptorTCP3.Methods.TCPServer
{
    class TCPServer
    {
        // Informs the main program that a log message is ready
        public delegate void LogEventHandler(string Message);
        public event LogEventHandler LogEvent;

        // Server Events
        public delegate void tcpConnectionClosedEventHandler();
        public event tcpConnectionClosedEventHandler tcpConnectionClosedEvent;
        public delegate void tcpLostConnectionEventHandler(string id);
        public event tcpLostConnectionEventHandler tcpLostConnectionEvent;
        public delegate void tcpSetLabelEventHandler(string lblName, string text);
        public event tcpSetLabelEventHandler tcpSetLabelEvent;

        NetComm.Host tcpServer = new Host(9119);

        Utilities.Utilities Utils = new Utilities.Utilities();
        Users.Users Users = new Users.Users();
        private RaptorTCP3.Methods.SystemURLS.sUrls URLS = new SystemURLS.sUrls();

        public TCPServer()
        {
            tcpServer.ConnectionClosed +=tcpServer_ConnectionClosed;
            tcpServer.DataReceived +=tcpServer_DataReceived;
            tcpServer.DataTransferred +=tcpServer_DataTransferred;
            tcpServer.errEncounter +=tcpServer_errEncounter;
            tcpServer.lostConnection +=tcpServer_lostConnection;
            tcpServer.onConnection +=tcpServer_onConnection;
            StartServerListening();
        }


        internal void StartServerListening()
        {          
            tcpServer.StartConnection();
            LogEvent("Server is Listening");
            Broadcast(RaptorTCP3.Methods.Enumerations.ServerCommands.Resume.ToString());  // Tell any waiting Clients they can resume communications.
        }

        internal void Broadcast(string Message)
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


        void tcpServer_onConnection(string id)
        {
            if (id.Contains("-")) 
            {
                LogEvent("Client Connecting");
                Users.allUsers.Add(id);
               
            }
        }

        void tcpServer_lostConnection(string id)
        {
            if (id.Length > 5)
            {
             
               Users.allUsers.Remove(id);
                try
                {
                    LogEvent("Client Disconnecting");
                    tcpLostConnectionEvent(id);
                    
                }
                catch (System.ObjectDisposedException de)
                {
                    LogEvent("Disposed... " + de.Message);
                }
                catch (Exception)
                {

                }
            }
            
        }

        void tcpServer_errEncounter(Exception ex)
        {
            LogEvent("Error: " + ex.Message);
        }

        void tcpServer_DataTransferred(string Sender, string Recipient, byte[] Data)
        {
            LogEvent("Incoming Message from: " + Sender + " to " + Recipient);
        }

        void tcpServer_DataReceived(string ID, byte[] Data)
        {
            string[] command = Utils.GetString(Data).Split(' ');
            switch (command[0].Trim().ToLowerInvariant())
            {
                case "Utils.Login":
                   
                    break;
                case "register":
                    LogEvent(ID + " Registering In");
                    if (RegistrationSuccessful(ID, Utils.GetString(Data)))
                        Reply(ID, command[0], RaptorTCP3.Methods.Enumerations.ServerCommands.Successful.ToString());
                    else
                        Reply(ID, command[0], RaptorTCP3.Methods.Enumerations.ServerCommands.Failed.ToString());
                    break;
                case "get":
                    LogEvent(ID + "Getting URLS");
                    SendUrls(ID, RaptorTCP3.Methods.Enumerations.ServerCommands.Successful.ToString(), GetURLS());
                    break;
                case "nop":
                    // if we receive this - which we shouldn't just ignore it.
                    break;
                default:
                    break;
            }
        }

        private void SendUrls(string ID, string sc, string[] urlList)
        {
            LogEvent("Sending URLS");
            string strBuffer = sc + " ";
            foreach (string url in urlList)
            {
                strBuffer += url + " ";
            }

            byte[] buffer = Utils.GetBytes(strBuffer);
            tcpServer.SendBufferSize = buffer.Length;
            string buffersize = RaptorTCP3.Methods.Enumerations.ServerCommands.SetMessageSize.ToString() + " " + buffer.Length.ToString();

            tcpServer.SendData(ID, Utils.GetBytes(buffersize));
            tcpServer.SendData(ID, buffer);
        }

        private string[] GetURLS()
        {
            LogEvent("Getting URLS to Send");
            string[] urlList = new string[10];

            for (int idx = 0; idx < 10; idx++)
            {
                string url = URLS.urlQueue.Dequeue().ToString();
                urlList[idx] = url;
                URLS.UpdateUrlInQueueStatus(url);
            }

            return urlList;
        }

        void tcpServer_ConnectionClosed()
        {
            lblConnections.Text = Users.allUsers.Count().ToString("N0");
            LogEvent("Connection Closed");
            tcpConnectionClosedEvent();
        }

        private void DisconnectAll()
        {
            LogEvent("Disconnecting all Clients");
            Parallel.ForEach(tcpServer.Users, ClientID =>
            {
                tcpServer.DisconnectUser(ClientID.ToString());
            });
        }
    }
}
