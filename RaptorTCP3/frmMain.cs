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
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace RaptorTCP3
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);
        delegate void SetLabelTextCallback(Label ctrl, string text);
        delegate void SetToolStripCallBack(string text);
        delegate void SetStatusLabelTextCallback(ToolStripStatusLabel ctrl, string text);

        private Task t;

        NetComm.Host tcpServer = new Host(9119);

        private static string connectionString = "Server=tcp:jy4i6onk8b.database.windows.net,1433;Database=Damocles;User ID=AtarashiNoDaveGordon@jy4i6onk8b;Password=P@r1n@zK0k@b1;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
        private SqlConnection con = new SqlConnection(connectionString);
        private bool sqlWorking = false;

        private string ClientLicense = null;

        private ObservableCollection<string> alClients = new ObservableCollection<string>();

        private Queue<string> urlQueue = new Queue<string>();

        private int SecondsPastSinceBoot;
        private int SecondsIdle;

        private bool IsIdle = true;

        public frmMain(bool ForceRegistration = false)
        {
            InitializeComponent();
            Log("Server Starting");
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

            Log("Exiting");
            if (tcpServer.Listening)
            {

                Broadcast(ServerCommands.Wait.ToString()); // Tell all the Clients to wait till I return
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
        #endregion

        #region Startup
        private void StartSQLClient()
        {
            alClients.CollectionChanged += alClients_CollectionChanged;

            Log("Starting SQL Client");
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

        void alClients_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (alClients.Count < 1)
                IsIdle = true;
            else
                IsIdle = false;
        }

        private int URLSCount()
        {
            Log("Counting URLS");
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM URLS");
            command.CommandType = CommandType.Text;
            command.Connection = con;
            int res = (int)command.ExecuteScalar();
            Log("Number of URLS in Database: " + res.ToString("N0"));
            return res;
        }

        private void PopulateURLQueue(int numberOfUrlsToGet)
        {
            Log("Populating URL Queue, Adding " + numberOfUrlsToGet);

           
                using (var db = new DamoclesEntities())
                {
                    var urls = db.URLS;
                    for (int idx = 0; idx < numberOfUrlsToGet; idx++)
                    {
                        var result = urls.FirstOrDefault(u => u.IsInProcessingQueue == false);
                        var urlp = result.URLPath;
                        urlQueue.Enqueue(urlp);
                        SetUrlToInProcessingQueue(urlp);
                        SetLabel(lblQueueLength, urlQueue.Count().ToString("N0"));
                        Application.DoEvents();
                    }
                }
                Log("Added  " + urlQueue.Count().ToString("N0") + "  to the Queue");

             }

        private void UpdateUrlInQueueStatus(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                using (var db = new DamoclesEntities())
                {
                    var urls = db.URLS;
                    var result = urls.FirstOrDefault(u => u.URLPath == url);
                    result.IsInProcessingQueue = true;
                    int rows = db.SaveChanges();
                    if (rows < 1) Log("Failed to Set Url to IsInProcessingQueue = True " + url);
                }
            }
}

        private void SetUrlToInProcessingQueue(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                using (var db = new DamoclesEntities())
                {
                    var urls = db.URLS;
                    var result = urls.FirstOrDefault(u => u.URLPath == url);
                    result.IsInProcessingQueue = true;
                    int rows = db.SaveChanges();
                    if (rows < 1) Log("Failed to Set Url to IsInProcessingQueue = True " + url);
                }
            }
}

        private void SubscribeToSQLEvents()
        {
            Log("Subscribing to SQL Events");
            con.Disposed += con_Disposed;
            con.StateChange += con_StateChange;
            con.InfoMessage += con_InfoMessage;
            con.FireInfoMessageEventOnUserErrors = true;

        }

        void con_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            StackFrame frame = new StackFrame(8);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            var name = method.Name;

            string m = e.Message;
            Log("SQL InfoMessage (" + name + ") " + e.Message);
        }

        void con_StateChange(object sender, StateChangeEventArgs e)
        {
            if (sqlWorking)
            {
                switch (con.State)
                {
                    case ConnectionState.Broken:
                        Log("SQL Connection Broken");
                        con.Close();
                        con.Open();
                        break;
                    case ConnectionState.Closed:
                        Log("SQL Connection Closed");
                        con.Open();
                        break;
                    case ConnectionState.Open:
                        Log("SQL Connection Open");
                        break;
                }
            }
        }

        void con_Disposed(object sender, EventArgs e)
        {
            Log("SQL Connection Disposed");
            sqlWorking = false;
        }

        private void StartServerListening()
        {
            Log("Server is Listening");
            tcpServer.StartConnection();
            Broadcast(ServerCommands.Resume.ToString());  // Tell any waiting Clients they can resume communications.
        }

        private void SubscribeToNetCommEvents()
        {
            Log("Subscribing to NetCom Events");
            tcpServer.ConnectionClosed += tcpServer_ConnectionClosed;
            tcpServer.DataReceived += tcpServer_DataReceived;
            tcpServer.DataTransferred += tcpServer_DataTransferred;
            tcpServer.errEncounter += tcpServer_errEncounter;
            tcpServer.lostConnection += tcpServer_lostConnection;
            tcpServer.onConnection += tcpServer_onConnection;

        }
        #endregion

        #region TCP Enums
        private enum ServerCommands
        {
            Successful,
            Failed,
            UseCache,
            Wait,
            Resume,
            SendEmailAddress,
            SetMessageSize,
        }

        private enum ClientCommands
        {
            Login,
            Register,
            Get,
            NOP,
        }
        #endregion

        #region Send and Reply TCP Messages
        private void Broadcast(string Message)
        {
            Log("Broadcasting: " + Message); ;
            tcpServer.Brodcast(GetBytes(Message));
        }

        private void Reply(string ID, string callingCommand, string Result)
        {
            Log("Replying to " + ID);
            string cr = callingCommand + " " + Result;
            byte[] commandResult = GetBytes(cr);
            tcpServer.SendData(ID, commandResult);

        }

        private void Reply(string ID, string callingCommand, string Result, string returnedValue)
        {
            Log("Replying to " + ID);
            string cr = callingCommand + " " + Result + " " + returnedValue;
            byte[] commandResult = GetBytes(cr);
            tcpServer.SendData(ID, commandResult);
        }
        #endregion

        #region TCP Events
        void tcpServer_onConnection(string id)
        {
            if (id.Length > 5)
            {
                Log("Client Connecting");
                alClients.Add(id);
                SetLabel(lblConnections, alClients.Count.ToString("N0"));
            }
        }

        void tcpServer_lostConnection(string id)
        {
            if (id.Length > 5)
            {
                SetLabel(lblConnections, alClients.Count.ToString("N0"));
                alClients.Remove(id);
                try
                {
                    Log("Client Disconnecting");
                    LogOffUser(id);
                }
                catch (System.ObjectDisposedException de)
                {
                    Log("Disposed... " + de.Message);
                }
                catch (Exception)
                {

                }
            }

        }

        void tcpServer_errEncounter(Exception ex)
        {
            Log("Error: " + ex.Message);
        }

        void tcpServer_DataTransferred(string Sender, string Recipient, byte[] Data)
        {
            Log("Incoming Message from: " + Sender + " to " + Recipient);
        }

        void tcpServer_DataReceived(string ID, byte[] Data)
        {
            string[] command = GetString(Data).Split(' ');
            switch (command[0].Trim().ToLowerInvariant())
            {
                case "login":
                    Log(ID + " Logging In");
                    if (LoginSuccessful(GetString(Data)))
                        Reply(ID, command[0], ServerCommands.Successful.ToString(), ClientLicense);
                    else
                        Reply(ID, command[0], ServerCommands.Failed.ToString());
                    break;
                case "register":
                    Log(ID + " Registering In");
                    if (RegistrationSuccessful(GetString(Data)))
                        Reply(ID, command[0], ServerCommands.Successful.ToString(), ClientLicense);
                    else
                        Reply(ID, command[0], ServerCommands.Failed.ToString());
                    break;
                case "get":
                    Log(ID + "Getting URLS");
                    SendUrls(ID, ServerCommands.Successful.ToString(), GetURLS());
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
            Log("Sending URLS");
            string strBuffer = sc + " ";
            foreach (string url in urlList)
            {
                strBuffer += url + " ";
            }

            byte[] buffer = GetBytes(strBuffer);
            tcpServer.SendBufferSize = buffer.Length;
            string buffersize = ServerCommands.SetMessageSize.ToString() + " " + buffer.Length.ToString();

            tcpServer.SendData(ID, GetBytes(buffersize));

            tcpServer.SendData(ID, buffer);

        }

        private string[] GetURLS()
        {
            Log("Getting URLS to Send");
            string[] urlList = new string[10];

            for (int idx = 0; idx < 10; idx++)
            {
                string url = urlQueue.Dequeue().ToString();
                urlList[idx] = url;
                UpdateUrlInQueueStatus(url);
            }

            return urlList;
        }





        void tcpServer_ConnectionClosed()
        {
            lblConnections.Text = alClients.Count().ToString("N0");
            Log("Connection Closed");
        }
        #endregion

        #region Licenses
        private string GenerateTemporaryLicenseNumber(string emailAddress)
        {
            string lNumberType = "t"; // temporary
            string lNumberAuthorisedFromYear = DateTime.UtcNow.Year.ToString();
            string lNumberCountryStateLanguageIDS = "zzz";
            string lNumberCounter = GetLicenseNumberCount();
            string ln = lNumberType + lNumberAuthorisedFromYear + lNumberCountryStateLanguageIDS + lNumberCounter;
            saveLicenseNumber(ln, emailAddress);
            return ln;
        }

        private void saveLicenseNumber(string ln, string emailAddress)
        {
            ClientLicense = ln;
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO LicenseNumbers VALUES('" + ln + "', N'" + emailAddress + "')");
                command.CommandType = CommandType.Text;
                command.Connection = con;
                command.ExecuteNonQuery();
            }
            catch (SqlException sqle)
            {
                Log("SQL ERROR: " + sqle.Message);
            }

        }

        private string GetLicenseNumberCount()
        {
            SqlCommand command = new SqlCommand("SELECT COUNT(Id) FROM LicenseNumbers");
            command.CommandType = CommandType.Text;
            command.Connection = con;
            int num = (int)command.ExecuteScalar();
            return (num + 1).ToString();
        }
        #endregion

        #region Login And Register

        private bool RegistrationSuccessful(string commandParams)
        {
            string[] command = commandParams.Trim().ToLowerInvariant().Split(' ');
            if (command[0] == "register")
            {
                return Register(command[1], command[2]);
            }
            return false;
        }

        private bool Register(string emailAddress, string Password)
        {

            // getutcdate() is SQL servers built in utc DateTime.UtcNow command

            SqlCommand command = null;

            command = new SqlCommand("INSERT INTO Users Values(N'" + emailAddress +
                           "','" + Password + "', getutcdate(), 3,2,4,1,'" + true + "'," + 2 + ",'" + GenerateTemporaryLicenseNumber(emailAddress) + "',N'" + emailAddress + "')");
            command.CommandType = CommandType.Text;
            command.Connection = con;
            int res = command.ExecuteNonQuery();
            if (res > 0)
            {
                // Update - create the LogonHistory table
                UpdateLoginHistory(emailAddress);

                return true;
            }
            else
                return false;

        }

        private void UpdateLoginHistory(string emailAddress)
        {
            // Get UserID from the Email Address
            // insert new record

            int uid = GetUserID(emailAddress);
            if (uid > 0)
            {

                // INSERT INTO LogonHistory VALUES(3,  getutcdate(),)
                SqlCommand command = new SqlCommand("INSERT INTO LogonHistory (UserId, LoggedOnDate) VALUES(" + uid + ",  getutcdate())");
                command.CommandType = CommandType.Text;
                command.Connection = con;
                command.ExecuteNonQuery();

            }
            else
            {
                Log("Error: (UpdateLoginHistory) Failed to getUuid");
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

            SqlCommand command = new SqlCommand("SELECT UserId from Users WHERE emailAddress = N'" + emailAddress + "' AND UserPasswordHash = '" + Password + "';");
            command.CommandType = CommandType.Text;
            command.Connection = con;
            int? uid = 0;

            uid = (int)command.ExecuteScalar();


            if (uid > 0)
            {
                // Set the User to IsOnline
                command = new SqlCommand("UPDATE Users SET IsOnline = '" + true + "' WHERE emailAddress = N'" + emailAddress + "' AND UserPasswordHash = '" + Password + "';");
                command.CommandType = CommandType.Text;
                command.Connection = con;
                int r = command.ExecuteNonQuery();
                if (r > 0) { Log("Updated User '" + emailAddress + "' to Online"); }

                // Update the User's Logon history - We will Update their LoggedOffDate when their TCP Connection is closed.
                command = new SqlCommand("INSERT INTO LogonHistory (LoggedOnDate) VALUES(getutcdate()) WHERE UserId=@UID;");
                command.Parameters.Add("@UID", SqlDbType.Int);
                command.Parameters["@UID"].Value = uid;
                command.CommandType = CommandType.Text;
                command.Connection = con;
                command.ExecuteNonQuery();
                return true;
            }
            return false;
        }

        private void LogOffUser(string id)
        {
            // Log the user off Users and Update LogonHistory
            if (id.Length > 5) // Ignore internal connections
            {

                Log(id + " Logging Off");
                SqlCommand command = new SqlCommand("UPDATE Users SET IsOnline = '" + false + "' WHERE UserId = '" + id + "';");
                command.CommandType = CommandType.Text;
                command.Connection = con;
                command.ExecuteNonQuery();
                command = new SqlCommand("UPDATE LogonHistory SET LoggedOffDate = getutcdate() WHERE UserId = '" + id + "' AND LoggedOffDate = '" + null + "';");
                command.CommandType = CommandType.Text;
                command.Connection = con;
                command.ExecuteNonQuery();
            }
        }

        private void LogOffAllUsers()
        {
            Log("Logging off all clients");

            SqlCommand command = new SqlCommand("UPDATE Users SET IsOnline = " + false + " WHERE IsOnline = " + false + "';");
            command.CommandType = CommandType.Text;
            command.Connection = con;
            int r = command.ExecuteNonQuery();

            command = new SqlCommand("UPDATE LogonHistory SET LoggedOffDate = " + DateTime.UtcNow + " WHERE LoggedOffDate = " + null + ";");
            command.CommandType = CommandType.Text;
            command.Connection = con;
            command.ExecuteNonQuery();

            Log("All users have been Logged Off");

        }
        #endregion

        #region Utilities

        private void DisconnectAll()
        {
            Log("Disconnecting all Clients");
            Parallel.ForEach(tcpServer.Users, ClientID =>
            {
                tcpServer.DisconnectUser(ClientID.ToString());
            });
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

        /// <summary>
        /// Logs the activities of both the TCP Server AND the SQL sub-system.
        /// </summary>
        /// <param name="message">
        /// String: Any message that needs to be displayed.
        /// </param>
        private void Log(string message)
        {
            string[] parts = message.Split(' ');
            if (parts[0].Length < 10 && parts[1].ToLowerInvariant().Trim() == "client")
            { }
            else if (parts[0].ToLowerInvariant().Trim() == "client")
            { }
            else
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
                        ConOut.Focus();
                    }
                }
                UpdateStatusMessage(message);
            }
        }

        private void UpdateStatusMessage(string message)
        {
            if (toolStripContainer1.InvokeRequired)
            {
                SetToolStripCallBack d = new SetToolStripCallBack(UpdateStatusMessage);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                lblStatus.Text = message;
            }
        }

        private void UpdateToolStripStatusLabel(ToolStripStatusLabel lbl, string message)
        {
            if (toolStripContainer1.InvokeRequired)
            {
                SetStatusLabelTextCallback d = new SetStatusLabelTextCallback(UpdateToolStripStatusLabel);
                this.Invoke(d, new object[] { lbl, message });
            }
            else
            {
                lbl.Text = message;
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

        #region SQL Utilities
        private int GetUserID(string emailAddress)
        {
            // SELECT UserId FROM Users WHERE emailAddress = N'dave@ccs-labs.com'
            SqlCommand command = new SqlCommand("SELECT UserId FROM Users WHERE emailAddress = N'" + emailAddress + "'");
            command.CommandType = CommandType.Text;
            command.Connection = con;
            int uid = (int)command.ExecuteScalar();
            return uid;

        }
        #endregion

        #region Form Events

        private void frmMain_Load(object sender, EventArgs e)
        {
            t = new Task(() => StartUp());
            t.Start();
        }

        private void StartUp()
        {
            Log("Application Loaded");
            // Subscribe to NetComm Events
            SubscribeToNetCommEvents();
            SubscribeToSQLEvents();
            // Start Listening
            StartServerListening();
            StartSQLClient();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log("Application Closing");
            LogOffAllUsers();
            Broadcast(ServerCommands.UseCache.ToString());
            t.Dispose();
            t.Wait();
        }
        #endregion

        #region Menu Events
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Delete ALL Database Data - 

            // Seed the Database Again
            SeedUrls();
        }
        #endregion

        private void seedUrlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SeedUrls();
        }

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
                this.Cursor = Cursors.WaitCursor;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Replace("\t", "").Trim();
                    if (line.StartsWith("http://") || line.StartsWith("https://"))
                    {
                        string[] parts = line.Split(' ');
                        string url = parts[0].Replace("\"", "").Trim();
                        if (!alUrls.Contains(url)) alUrls.Add(url);
                    }
                    Application.DoEvents();
                }
                this.Cursor = Cursors.Default;
                sr.Close();
                fs.Close();


                SaveUrls(alUrls);
            }
            catch (Exception ex)
            {
                Log("Error Loading Seed Urls (seedUrlsToolStripMenuItem_Click) " + ex.Message);
                if (sr != null) sr.Close();
                if (fs != null) fs.Close();
            }
        }

        private static string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }

        private void SaveUrls(ArrayList alUrls)
        {
            Log("Seeding URLS");
            int rows = 0;
            this.Cursor = Cursors.WaitCursor;
            Progress.Maximum = alUrls.Count;
            SqlCommand command = new SqlCommand("DELETE FROM URLS");
            command.Connection = con;
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            int idx = 0;
            foreach (string url in alUrls)
            {
                idx++;
                Progress.Value = idx;
                string newurl = DecodeUrlString(url);


                command =
                   new SqlCommand("INSERT INTO URLS (UrlHash, URLPath, DiscoveredById, DiscoveryDate, IsInProcessingQueue) " +
                       " VALUES(@URLHASH,@URLPATH, @DISCOVEREDBYID, @DISCOVERYDATE, @ISINPROCESSINGQUEUE)");

                command.Parameters.Add("@URLHASH", SqlDbType.NVarChar);
                command.Parameters["@URLHASH"].Value = HashPassword(newurl);
                command.Parameters.Add("@URLPATH", SqlDbType.NVarChar);
                command.Parameters["@URLPATH"].Value = newurl;
                command.Parameters.Add("@DISCOVEREDBYID", SqlDbType.Int);
                command.Parameters["@DISCOVEREDBYID"].Value = 1006;
                command.Parameters.Add("@DISCOVERYDATE", SqlDbType.DateTime);
                command.Parameters["@DISCOVERYDATE"].Value = DateTime.UtcNow;
                command.Parameters.Add("@ISINPROCESSINGQUEUE", SqlDbType.Bit);
                command.Parameters["@ISINPROCESSINGQUEUE"].Value = false;

                command.CommandType = CommandType.Text;
                command.Connection = con;
                rows += command.ExecuteNonQuery();

                Application.DoEvents();
            }
            Progress.Value = 0;
            this.Cursor = Cursors.Default;
            Log("Seeded URLS Table with " + rows.ToString("N0") + " rows");
            alUrls.Clear();
        }

        private void timerOneSecond_Tick(object sender, EventArgs e)
        {
            SecondsPastSinceBoot++;

            if (IsIdle)
                SecondsIdle++;
            else
                SecondsIdle = 0;


            UpdateToolStripStatusLabel(lblRuntime, SecondsToDHMS(SecondsPastSinceBoot).ToString());
            UpdateToolStripStatusLabel(lblIdleTime, SecondsToDHMS(SecondsIdle).ToString());
        }

        private string SecondsToDHMS(int Seconds)
        {
            return TimeSpan.FromSeconds(Seconds).ToString();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
