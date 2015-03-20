using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RaptorTCP3.Methods.Utilities;
using System.Windows.Forms;
using RaptorTCP3.Methods.Users;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace RaptorTCP3.Methods.TCPServer
{
    //class TCPServer
    //{
    //    // Informs the main program that a log message is ready
    //    public delegate void LogEventHandler(string Message);
    //    public event LogEventHandler LogEvent;

    //    // Server Events
    //    public delegate void tcpConnectionClosedEventHandler();
    //    public event tcpConnectionClosedEventHandler tcpConnectionClosedEvent;
    //    public delegate void tcpLostConnectionEventHandler(string id);
    //    public event tcpLostConnectionEventHandler tcpLostConnectionEvent;
    //    public delegate void tcpConnectionEventHandler(string id);
    //    public event tcpConnectionEventHandler tcpConnectionEvent;

    //    public bool Listening { get; set; }

    //    NetComm.Host tcpServer = new Host(9119);
    //    Utilities.Utilities Utils = new Utilities.Utilities();

    //    Users.Users Users = new Users.Users();
    //    private RaptorTCP3.Methods.SystemURLS.sUrls URLS = new SystemURLS.sUrls();
    //    RaptorTCP3.Methods.Registration.Registration Registration = new Registration.Registration();

    //    public TCPServer()
    //    {
    //        tcpServer.ConnectionClosed += tcpServer_ConnectionClosed;
    //        tcpServer.DataReceived += tcpServer_DataReceived;
    //        tcpServer.DataTransferred += tcpServer_DataTransferred;
    //        tcpServer.errEncounter += tcpServer_errEncounter;
    //        tcpServer.lostConnection += tcpServer_lostConnection;
    //        tcpServer.onConnection += tcpServer_onConnection;
    //        StartServerListening();
    //    }


    //    internal void StartServerListening()
    //    {
    //        tcpServer.StartConnection();
    //        Listening = true;
    //        if (LogEvent != null) LogEvent("Server is Listening");
    //        Broadcast(RaptorTCP3.Methods.Enumerations.ServerCommands.Resume.ToString());  // Tell any waiting Clients they can resume communications.
    //    }

    //    internal void Broadcast(string Message)
    //    {
    //        if (LogEvent != null) LogEvent("Broadcasting: " + Message);
    //        if (Users.allUsers.Count() > 0) tcpServer.Brodcast(Utils.GetBytes(Message));
    //    }

    //    internal void Reply(string ID, string callingCommand, string Result)
    //    {
    //        if (LogEvent != null) LogEvent("Replying to " + ID);
    //        string cr = callingCommand + " " + Result;
    //        byte[] commandResult = Utils.GetBytes(cr);
    //        tcpServer.SendData(ID, commandResult);
    //    }

    //    internal void Reply(string ID, string callingCommand, string Result, string returnedValue)
    //    {
    //        if (LogEvent != null) LogEvent("Replying to " + ID);
    //        string cr = callingCommand + " " + Result + " " + returnedValue;
    //        byte[] commandResult = Utils.GetBytes(cr);
    //        tcpServer.SendData(ID, commandResult);
    //    }


    //    void tcpServer_onConnection(string id)
    //    {
    //        if (id.Contains("-"))
    //        {
    //            if (LogEvent != null) LogEvent("Client Connecting");
    //            // Users.allUsers.Add(id); Changed to an Event!
    //            tcpConnectionEvent(id);
    //        }
    //    }

    //    void tcpServer_lostConnection(string id)
    //    {
    //        if (id.Length > 5)
    //        {
    //            Users.allUsers.Remove(id);
    //            try
    //            {
    //                if (LogEvent != null) LogEvent("Client Disconnecting");
    //                tcpLostConnectionEvent(id);

    //            }
    //            catch (System.ObjectDisposedException de)
    //            {
    //                if (LogEvent != null) LogEvent("Disposed... " + de.Message);
    //            }
    //            catch (Exception)
    //            {

    //            }
    //        }
    //    }

    //    void tcpServer_errEncounter(Exception ex)
    //    {
    //        if (LogEvent != null) LogEvent("Error: " + ex.Message);
    //    }

    //    void tcpServer_DataTransferred(string Sender, string Recipient, byte[] Data)
    //    {
    //        if (LogEvent != null) LogEvent("Incoming Message from: " + Sender + " to " + Recipient);
    //    }

    //    void tcpServer_DataReceived(string ID, byte[] Data)
    //    {
    //        if (LogEvent != null) LogEvent(ID + " Sent Data");
    //        string[] command = Utils.GetString(Data).Split(' ');
    //        switch (command[0].Trim().ToLowerInvariant())
    //        {
    //            case "login":
    //                if (LogEvent != null) LogEvent(ID + " Attempting Login");
    //                break;
    //            case "register":
    //                if (LogEvent != null) LogEvent(ID + " Attempting Registration");
    //                if (Registration.RegistrationSuccessful(ID, Utils.GetString(Data)))
    //                    Reply(ID, command[0], RaptorTCP3.Methods.Enumerations.ServerCommands.Successful.ToString());
    //                else
    //                    Reply(ID, command[0], RaptorTCP3.Methods.Enumerations.ServerCommands.Failed.ToString());
    //                break;
    //            case "get":
    //                if (LogEvent != null) LogEvent(ID + " Wants More URLS");
    //                SendUrls(ID, RaptorTCP3.Methods.Enumerations.ServerCommands.Successful.ToString(), GetURLS());
    //                break;
    //            case "nop":
    //                // if we receive this - which we shouldn't, just ignore it.
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //    internal void SendWait()
    //    {
    //        if (LogEvent != null) LogEvent("Broadcasting Wait");
    //        if (Users.allUsers.Count() > 0)
    //            tcpServer.Brodcast(Utils.GetBytes(RaptorTCP3.Methods.Enumerations.ServerCommands.Wait.ToString()));
    //    }

    //    internal void SendResume()
    //    {
    //        if (LogEvent != null) LogEvent("Broadcasting Resume");
    //        if (Users.allUsers.Count() > 0)
    //            tcpServer.Brodcast(Utils.GetBytes(RaptorTCP3.Methods.Enumerations.ServerCommands.Resume.ToString()));
    //    }

    //    private void SendUrls(string ID, string sc, string[] urlList)
    //    {
    //        if (LogEvent != null) LogEvent(ID + " Being Sent URLS");
    //        string strBuffer = sc + " ";
    //        foreach (string url in urlList)
    //        {
    //            strBuffer += url + " ";
    //        }

    //        byte[] buffer = Utils.GetBytes(strBuffer);
    //        tcpServer.SendBufferSize = buffer.Length;
    //        string buffersize = RaptorTCP3.Methods.Enumerations.ServerCommands.SetMessageSize.ToString() + " " + buffer.Length.ToString();

    //        tcpServer.SendData(ID, Utils.GetBytes(buffersize));
    //        tcpServer.SendData(ID, buffer);
    //    }

    //    private string[] GetURLS()
    //    {
    //        if (LogEvent != null) LogEvent("Getting URLS to Send");
    //        string[] urlList = new string[10];

    //        for (int idx = 0; idx < 10; idx++)
    //        {
    //            string url = URLS.urlQueue.Dequeue().ToString();
    //            urlList[idx] = url;
    //            URLS.UpdateUrlInQueueStatus(url);
    //        }
    //        return urlList;
    //    }

    //    void tcpServer_ConnectionClosed()
    //    {
    //        if (LogEvent != null) LogEvent("Connection Closed");
    //        Listening = false;
    //        tcpConnectionClosedEvent();
    //    }

    //    internal void DisconnectAll()
    //    {
    //        if (LogEvent != null) LogEvent("Disconnecting all Clients");
    //        Parallel.ForEach(tcpServer.Users, ClientID =>
    //        {
    //            tcpServer.DisconnectUser(ClientID.ToString());
    //        });
    //    }



    //    internal void SendWait(string id)
    //    {
    //        if (LogEvent != null) LogEvent(id + " Told to Wait");
    //        tcpServer.SendData(id, Utils.GetBytes(RaptorTCP3.Methods.Enumerations.ServerCommands.Wait.ToString()));
    //    }

    //    internal void SendResume(string id)
    //    {
    //        if (LogEvent != null) LogEvent(id + " Told to Resume");
    //        tcpServer.SendData(id, Utils.GetBytes(RaptorTCP3.Methods.Enumerations.ServerCommands.Resume.ToString()));
    //    }
    //}

    class TCPServerWCF
    {

    }
}
