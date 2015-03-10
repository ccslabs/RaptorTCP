using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;

using RaptorTCP3.Forms;
using RaptorTCP3.Methods.Utilities;
using RaptorTCP3.Methods.Login;
using RaptorTCP3.Methods.TCPServer;
using RaptorTCP3.Methods.SystemURLS;
using RaptorTCP3.Methods;

namespace RaptorTCP3
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);
        delegate void SetLabelTextCallback(Label ctrl, string text);
        delegate void SetToolStripCallBack(string text);
        delegate void SetStatusLabelTextCallback(ToolStripStatusLabel ctrl, string text);

        private Utilities Utils = new Utilities();
        private LoginMethods LoginMethods = new LoginMethods();
        private Seeding Seeding = new Seeding();
        private TCPServer tcpServer = new TCPServer();
        private sUrls systemUrls = new sUrls();
        private Task t;

       

        private static string connectionString = Properties.Settings.Default.DamoclesConnectionString;


        private string ClientLicense = null;

       

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

            LoginMethods.LogEvent += LogEvent;
            LoginMethods.LoginResultEvent += LoginMethods_LoginResultEvent;

            systemUrls.LogEvent += LogEvent;
            systemUrls.UrlsCountResultEvent += systemUrls_UrlsCountResultEvent;
            systemUrls.UrlsToEnqueueEvent += systemUrls_UrlsToEnqueueEvent;

            tcpServer.LogEvent += LogEvent;

            Seeding.LogEvent += LogEvent;
            Seeding.ProgressChangedEvent += Seeding_ProgressChangedEvent;
            Seeding.ProgressMaximumChangedEvent += Seeding_ProgressMaximumChangedEvent;

            allUsers.CollectionChanged += allUsers_CollectionChanged;
        }

        void allUsers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (allUsers.Count < 1)
                IsIdle = true;
            else
                IsIdle = false;
        }

 
        #region Seeding Events
        void Seeding_ProgressMaximumChangedEvent(int Max)
        {
            Progress.Maximum = Max;
        }

        void Seeding_ProgressChangedEvent(int Value)
        {
            Progress.Value = Value;
        }
        #endregion
       
        #region URL Events
        void systemUrls_UrlsToEnqueueEvent(string URL)
        {
            urlQueue.Enqueue(URL);
            SetLabel(lblQueueLength, urlQueue.Count.ToString("N0"));
        }

        void systemUrls_UrlsCountResultEvent(long Result)
        {
            Log("Total URLS: " + Result);
        }
        #endregion

        #region Login Events
        void LoginMethods_LoginResultEvent(bool Result, string Cid)
        {
            Log(Cid + " Is Attempting a Login");
            if (Result)
                Reply(Cid, RaptorTCP3.Methods.Enumerations.ClientCommands.Login.ToString(), RaptorTCP3.Methods.Enumerations.ServerCommands.Successful.ToString(), ClientLicense);
            else
                Reply(Cid, RaptorTCP3.Methods.Enumerations.ClientCommands.Login.ToString(), RaptorTCP3.Methods.Enumerations.ServerCommands.Failed.ToString());
        }

        void LogEvent(string Message)
        {
            Log(Message);
        }
        #endregion



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

        private bool RegistrationSuccessful(string ID, string commandParams)
        {
            string[] command = commandParams.Trim().ToLowerInvariant().Split(' ');
            if (command[0] == "register")
            {
                return Register(ID, command[1], command[2]);
            }
            return false;
        }

        private bool Register(string ClientID, string emailAddress, string Password)
        {

            using (var db = new DamoclesEntities())
            {
                System.Data.Entity.DbSet<User> users = db.Users;

                var eu = CreateUser(ClientID, emailAddress, Password);
                users.Add(eu);

                int rows = db.SaveChanges();
                if (rows < 1)
                {
                    Log("Failed to Add user: " + emailAddress);
                    return false;
                }
                else
                {
                    UpdateLoginHistory(emailAddress);
                    return true;
                }
            }
        }

        private User CreateUser(string ClientID, string emailAddress, string Password)
        {
            var eu = new User();
            eu.Username = emailAddress;
            eu.UserPasswordHash = Password;
            eu.RegisteredDate = DateTime.UtcNow;
            eu.CountryId = 3;
            eu.StateId = 2;
            eu.JurisidictionId = 4;
            eu.LanguagesId = 1;
            eu.LicenseNumber = GenerateTemporaryLicenseNumber(emailAddress);
            eu.emailAddress = emailAddress;
            //TODO: ADD TRACKING OF THE VARIOUS CLIENTS THE USER HAS
            //  eu.UserClientID =  AddClient(ClientID, emailAddress);
            return eu;
        }







        private void LogOffUser(string Cid)
        {
            //TODO: We need to store the User Clients IDs when they Utils.Log on so we can Utils.Log them off properly at the end!
            if (Cid.Contains("-")) // Internally generated IDs do not contains hyphens.
            {

                using (var db = new DamoclesEntities())
                {
                    var user = db.Users.First(u => u.CurrentClientID  == Cid);
                    user.IsOnline = false;
                    int rows = db.SaveChanges();
                    string emailAddress = GetUserEmailAddressByID(Cid);
                    UpdateLogOffHistory(emailAddress);
                    if (rows == 1)
                    {
                        LogemailAddress + " is Utils.Logged In");

                    }
                    else
                    {
                        LogemailAddress + " Failed to Utils.Log In");

                    }
                }
            }
        }

        private void UpdateLogOffHistory(string emailAddress)
        {
            using (var db = new DamoclesEntities())
            {
                var uid = GetUserID(emailAddress);
                var loh = db.LogonHistories.First(lh => lh.LoggedOffDate == null && lh.UserId == uid);
                loh.LoggedOffDate = DateTime.UtcNow;
                db.SaveChanges();

                var user = db.Users.First(u => u.UserId == uid);
                user.IsOnline = false;
                user.CurrentClientID = null;
                db.SaveChanges();
            }
        }

        private string GetUserEmailAddressByID(string Cid)
        {
            using (var db = new DamoclesEntities())
            {
                var user = db.Users.First(u => u.CurrentClientID == Cid && u.IsOnline == true);
                return user.emailAddress;
            }
        }

        private void LogOffAllUsers()
        {
            Log("Utils.Logging off all clients");

            foreach (string Cid in allUsers)
            {
                LogOffUser(Cid);
            }

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

      

      
        #endregion

        #region SQL Utilities
        private int GetUserID(string emailAddress)
        {
            using (var db = new DamoclesEntities())
            {
                var user = db.Users.First(u => u.emailAddress == emailAddress);
                return user.UserId;
            }
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
            Utils.LogOffAllUsers();
            Broadcast(ServerCommands.UseCache.ToString());
            t.Dispose();
            t.Wait();
        }
        #endregion

        #region Menu Events

        #endregion

        private void seedUrlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Seeding.SeedUrls();
        }



       

       

        private static void DeleteAllURLS(DamoclesEntities db)
        {
            var all = from c in db.URLS select c;
            db.URLS.RemoveRange(all);
            db.SaveChanges();
        }

        private URL AddUrl(string url)
        {
            URL ue = new URL();
            ue.UrlHash = Utils.HashPassword(url);
            ue.URLPath = url;
            ue.DiscoveredById = 1006;
            ue.DiscoveryDate = DateTime.UtcNow;
            ue.IsInProcessingQueue = false;
            return ue;
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

        private void fullResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SeedUsers();
            SeedUrls();
        }

        private void SeedUsers()
        {


            using (var db = new DamoclesEntities())
            {
                DeleteAllUsers(db);

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
            su.emailAddress = em;
            su.UserPasswordHash = "097dfd905dfa0e078883b7afcf7e653dde569bb1ed2ce3384d9c9ed7b85741d6e8d1b1a356318805d3c8b31b36a9916936d005d8134fb015d0392cf75cd7fa24";
            su.RegisteredDate = DateTime.UtcNow;
            su.CountryId = 3;
            su.StateId = 2;
            su.JurisidictionId = 4;
            su.LanguagesId = 1;
            su.IsOnline = false;
            su.AccountStatusId = 3;
            su.LicenseNumber = GenerateTemporaryLicenseNumber(em);
            su.emailAddress = em;
            //su.UserClientID = null;
            //su.CurrentClientID = null;
            usrs.Add(su);
            db.SaveChanges();
        }

        private void SeedAdminUser(DamoclesEntities db, System.Data.Entity.DbSet<User> usrs, User su, string em)
        {
            su.emailAddress = em;
            su.UserPasswordHash = "097dfd905dfa0e078883b7afcf7e653dde569bb1ed2ce3384d9c9ed7b85741d6e8d1b1a356318805d3c8b31b36a9916936d005d8134fb015d0392cf75cd7fa24";
            su.RegisteredDate = DateTime.UtcNow;
            su.CountryId = 3;
            su.StateId = 2;
            su.JurisidictionId = 4;
            su.LanguagesId = 1;
            su.IsOnline = false;
            su.AccountStatusId = 3;
            su.LicenseNumber = GenerateTemporaryLicenseNumber(em);
            su.emailAddress = em;
            su.UserClientID = null;
            su.CurrentClientID = null;
            usrs.Add(su);
            db.SaveChanges();
        }

        private static void DeleteAllUsers(DamoclesEntities db)
        {
            var all = from c in db.Users select c;
            db.Users.RemoveRange(all);
            db.SaveChanges();
        }

        /// <summary>
        /// Show all the Registered users
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowUsers showUsers = new frmShowUsers();
            showUsers.Show();
        }


        /// <summary>
        /// Logs the activities of both the TCP Server AND the SQL sub-system.
        /// </summary>
        /// <param name="message">
        /// String: Any message that needs to be displayed.
        /// </param>
        internal void Log(string message)
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

    }
}
