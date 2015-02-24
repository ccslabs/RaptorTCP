using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NetComm;

namespace RaptorTCP3
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);
        delegate void SetLabelTextCallback(Label ctrl, string text);
        NetComm.Host tcpServer = new Host(9119);

        public frmMain()
        {
            InitializeComponent();
            panel1.Top = 0;
            panel1.Left = 0;
            panel1.Width = this.Width;
            panel1.Height = this.Height / 3;

            ConOut.Left = 0;
            ConOut.Top = this.Height / 3;
            ConOut.Width = this.Width;
            ConOut.Height = 2 * this.Height / 3;
        }

        #region cmsSystem Operations

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tcpServer.Listening)
            {
                Broadcast("Wait"); // Tell all the Clients to wait till I return
                DisconnectAll();
                tcpServer.CloseConnection();

            }
            Application.Exit();
        }

        private void DisconnectAll()
        {
            Parallel.ForEach(tcpServer.Users, ClientID =>
            {
                tcpServer.DisconnectUser(ClientID.ToString());
            });

            //foreach(string ClientID in tcpServer.Users)
            //{
            //    tcpServer.DisconnectUser(ClientID);
            //}
        }
        #endregion

        #region fmrMain Events
        private void frmMain_Load(object sender, EventArgs e)
        {
            // Subscribe to NetComm Events
            SubscribeToNetCommEvents();

            // Start Listening
            StartServerListening();

        }

        private void StartServerListening()
        {
            Log("Server is Listening");
            tcpServer.StartConnection();
            Broadcast("Resume");  // Tell any waiting Clients they can resume communications.
        }

        private void SubscribeToNetCommEvents()
        {
            tcpServer.ConnectionClosed += tcpServer_ConnectionClosed;
            tcpServer.DataReceived += tcpServer_DataReceived;
            tcpServer.DataTransferred += tcpServer_DataTransferred;
            tcpServer.errEncounter += tcpServer_errEncounter;
            tcpServer.lostConnection += tcpServer_lostConnection;
            tcpServer.onConnection += tcpServer_onConnection;

        }

        private void Broadcast(string Message)
        {
            tcpServer.Brodcast(GetBytes(Message));
        }

        void tcpServer_onConnection(string id)
        {
            SetLabel(lblConnections, tcpServer.Users.Count().ToString("N0"));
            Log(id + " Connected");
        }

       
        private void SetLabel(Label lbl, string message)
        {
            if (lbl.InvokeRequired)
            {
                SetLabelTextCallback d = new SetLabelTextCallback(SetLabel);
                this.Invoke(d, new object[] { lbl, message });
            }
            else
            {
                lbl.Text = message;
            }
        }



        void tcpServer_lostConnection(string id)
        {
            SetLabel(lblConnections, tcpServer.Users.Count().ToString("N0"));
            try
            {
                Log(id + " Disconnected");
            }
            catch (System.ObjectDisposedException de)
            {
                Log("Disposed... " + de.Message);
            }
            catch (Exception)
            {

            }
        }

        void tcpServer_errEncounter(Exception ex)
        {
            Log("Error: " + ex.Message);
        }

        void tcpServer_DataTransferred(string Sender, string Recipient, byte[] Data)
        {
            Log("Message Transfer Requested by " + Sender + " to " + Recipient + " Message = " + GetString(Data));
        }

        private enum ServerCommands
        {
            Successful,
            Failed,
        }

        void tcpServer_DataReceived(string ID, byte[] Data)
        {
            string[] command = GetString(Data).Split(' ');
            Log(ID + " Sent this Data: " + GetString(Data));

            switch (command[0].Trim().ToLowerInvariant())
            {
                case "login":
                    if (LoginSuccessful(GetString(Data)))
                        Reply(ID, command[0], ServerCommands.Successful.ToString());
                    else
                        Reply(ID, command[0], ServerCommands.Failed.ToString());
                    break;
                case "register":
                    break;
                default:
                    break;
            }
        }

        private bool LoginSuccessful(string commandParams)
        {
            string[] command = commandParams.Trim().ToLowerInvariant().Split(' ');
            if (command[0] == "login")
            {
                return Login(command[1], command[2]);
            }
            return false;
        }

        private bool Login(string p1, string p2)
        {
            return false;
        }

        private void Reply(string ID, string callingCommand, string Result)
        {
            //string cr = callingCommand + " " + Result;
            //byte[] commandResult = GetBytes(cr);
            //tcpServer.SendData(ID, commandResult);

        }

        void tcpServer_ConnectionClosed()
        {
            lblConnections.Text = tcpServer.Users.Count().ToString("N0");
            Log("Connection Closed");
        }

        #endregion

        #region Utilities

        private void Log(string message)
        {
            if (!ConOut.IsDisposed)
            {
                if (this.ConOut.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(Log);
                    this.Invoke(d, new object[] { message });
                }
                else
                {
                    ConOut.AppendText(DateTime.Now + "\t" + message + Environment.NewLine);
                }
            }
        }

        private string GetString(byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        private byte[] GetBytes(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }
        #endregion
    }
}
