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
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace RaptorTCP3
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);
        delegate void SetLabelTextCallback(Label ctrl, string text);

        NetComm.Host tcpServer = new Host(9119);

        private static string connectionString = "Server=tcp:jy4i6onk8b.database.windows.net,1433;Database=Damocles;User ID=AtarashiNoDaveGordon@jy4i6onk8b;Password=P@r1n@zK0k@b1;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
        private SqlConnection con = new SqlConnection(connectionString);
        private bool sqlWorking = false;
        private static int ClientCount = 1;


        public frmMain(bool ForceRegistration = false)
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

            if (con.State != ConnectionState.Closed)
            {
                while (sqlWorking)
                {
                    System.Threading.Thread.Sleep(2000);
                }
                con.Close();
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
            SubscribeToSQLEvents();
            // Start Listening
            StartServerListening();
            StartSQLClient();
        }

        private void StartSQLClient()
        {
            con.Open();
        }

        private void SubscribeToSQLEvents()
        {
            con.Disposed += con_Disposed;
            con.InfoMessage += con_InfoMessage;
            con.StateChange += con_StateChange;
            con.FireInfoMessageEventOnUserErrors = true;

        }

        void con_StateChange(object sender, StateChangeEventArgs e)
        {
            switch (con.State)
            {
                case ConnectionState.Broken:
                    con.Close();
                    con.Open();
                    break;
                case ConnectionState.Closed:
                    con.Open();
                    break;
            }
        }

        void con_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Log("SQL ERROR: " + e.Message);
        }

        void con_Disposed(object sender, EventArgs e)
        {
            sqlWorking = false;
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

        private bool Login(string emailAddress, string Password)
        {
            // Does the User Exist in the Database?
            SqlCommand command = new SqlCommand("SELECT UserId from Users WHERE emailAddress = N'" + emailAddress + "' AND UserPasswordHash = " + HashPassword(Password) + ";");
            int uid = (int)command.ExecuteScalar();

            if (uid != 0)
            {
                // Set the User to IsOnline
                command = new SqlCommand("UPDATE Users SET IsOnline = " + true + " WHERE emailAddress = N'" + emailAddress + "' AND UserPasswordHash = " + HashPassword(Password) + ";");
                int r = command.ExecuteNonQuery();
                if (r > 0) { Log("Updated User " + emailAddress + " to Online"); }

                // Update the User's Logon history - We will Update their LoggedOffDate when their TCP Connection is closed.
                command = new SqlCommand("UPDATE LogonHistory SET LoggedOnDate = " + DateTime.UtcNow + " WHERE UserId = " + uid + ";");
                command.ExecuteNonQuery();
                return true;
            }
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


        /// <summary>
        /// Logs the activities of both the TCP Server AND the SQL sub-system.
        /// </summary>
        /// <param name="message">
        /// String: Any message that needs to be displayed.
        /// </param>
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

        /// <summary>
        /// Converts A byte array to a string
        /// </summary>
        /// <param name="data">
        /// Byte[]: The Byte Array to convert
        /// </param>
        /// <returns>
        /// String: The results of the conversion
        /// </returns>
        private string GetString(byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// Converts a string to a Byte Array
        /// </summary>
        /// <param name="data">
        /// String: The text to convert
        /// </param>
        /// <returns>
        /// Byte[]: The result of the conversion
        /// </returns>
        private byte[] GetBytes(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        /// <summary>
        /// The SHA512 Method for hashing passwords before they are stored in the Database
        /// </summary>
        /// <param name="Password">
        /// string: The string representation of the password
        /// </param>
        /// <returns>
        /// String: A Hexadecimal string of 128 characters which represent the password.
        /// </returns>
        /// <remarks>
        /// The Client should send a Hashed Password instead of a plain text password.
        /// </remarks>
        private string HashPassword(string Password)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                byte[] hash = shaM.ComputeHash(GetBytes(Password));
                return GetHex(hash);
            }

        }

        /// <summary>
        /// Converts a Byte Array to a Hexadecimal string
        /// </summary>
        /// <param name="hash">
        /// Byte[]: The byte array of the password hash to convert to a Hexadecimal string
        /// </param>
        /// <returns>
        /// String: The Hexadecimal string that results from the conversion. 128 characters long.
        /// </returns>
        private string GetHex(byte[] hash)
        {
            string hex = BitConverter.ToString(hash);
            return hex.Replace("-", "");
        }
        #endregion
    }
}
