﻿using System;
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
using RaptorTCP3.Methods.LogOff;
using RaptorTCP3.Methods.TCPServer;
using RaptorTCP3.Methods.SystemURLS;
using RaptorTCP3.Methods.SqlClient;
using RaptorTCP3.Methods;
using RaptorTCP3.Methods.Users;

namespace RaptorTCP3
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);
        delegate void SetLabelTextCallback(Label ctrl, string text);
        delegate void SetToolStripCallBack(string text);
        delegate void SetStatusLabelTextCallback(ToolStripStatusLabel ctrl, string text);

        delegate void SetToolStripProgressMaximumCallBack(int value);
        delegate void SetToolStripProgressValueCallBack(int value);
        


        private Utilities Utils = new Utilities();
        private LoginMethods LoginMethods = new LoginMethods();
        private LogOff Logoff = new LogOff();
        private Seeding Seeding = new Seeding();
        private TCPServer tcpServer = new TCPServer();
        private sUrls URLS = new sUrls();
        private Users Users = new Users();
        private SqlClient DatabaseClient = new SqlClient();

        private Task t;

        private static string connectionString = Properties.Settings.Default.DamoclesConnectionString;

        private string ClientLicense = null;

        private int SecondsPastSinceBoot;
        private int SecondsIdle;

        private bool IsIdle = true;
        private bool IsInWaitMode = false;

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
            ConOut.Height = (2 * this.Height / 3) - 50;

           
        }

        private void StartUp()
        {
            Log("Subscribing to Login Events");
            LoginMethods.LogEvent += LogEvent;
            LoginMethods.LoginResultEvent += LoginMethods_LoginResultEvent;

            Log("Subscribing to URL Events");
            URLS.LogEvent += LogEvent;
            URLS.UrlsCountResultEvent += systemUrls_UrlsCountResultEvent;
            URLS.NoUrlsLeftToProcessEvent += URLS_NoUrlsLeftToProcessEvent;
            URLS.MoreUrlsLeftToProcessEvent += URLS_MoreUrlsLeftToProcessEvent;
            URLS.ProgressChangedEvent += ProgressChangedEvent;
            URLS.ProgressMaximumChangedEvent += ProgressMaximumChangedEvent;

            Log("Subscribing to TCP  Server Events");
            tcpServer.LogEvent += LogEvent;
            tcpServer.tcpConnectionClosedEvent += tcpServer_tcpConnectionClosedEvent;
            tcpServer.tcpLostConnectionEvent += tcpServer_tcpLostConnectionEvent;

            Log("Subscribing to Seeding Events");
            Seeding.LogEvent += LogEvent;
            Seeding.ProgressChangedEvent += ProgressChangedEvent;
            Seeding.ProgressMaximumChangedEvent += ProgressMaximumChangedEvent;

            Log("Subscribing to Database Client Events");
            DatabaseClient.LogEvent += LogEvent;
            Log("Application Loaded");
            Log("Populating URL Queue");
            URLS.PopulateURLQueue(50);

        }

        #region tcpServer Events
        void tcpServer_tcpLostConnectionEvent(string id)
        {
            Logoff.LogOffUser(id);
        }



        void tcpServer_tcpConnectionClosedEvent()
        {
            lblConnections.Text = Users.allUsers.Count().ToString("N0");

        }

        #endregion
      
        #region URL Events
        void systemUrls_UrlsToEnqueueEvent(string URL)
        {
            URLS.urlQueue.Enqueue(URL);
            SetLabel(lblQueueLength, URLS.urlQueue.Count.ToString("N0"));
        }

        void URLS_MoreUrlsLeftToProcessEvent()
        {
            IsInWaitMode = false;
            SetLabel(lblWaitStatus, "Resumed");
            tcpServer.SendResume();
        }

        void URLS_NoUrlsLeftToProcessEvent()
        {
            IsInWaitMode = true;
            SetLabel(lblWaitStatus, "Waiting");
            tcpServer.SendWait(); // Tell all Connected clients to wait!
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
                tcpServer.Reply(Cid, RaptorTCP3.Methods.Enumerations.ClientCommands.Login.ToString(), RaptorTCP3.Methods.Enumerations.ServerCommands.Successful.ToString(), ClientLicense);
            else
                tcpServer.Reply(Cid, RaptorTCP3.Methods.Enumerations.ClientCommands.Login.ToString(), RaptorTCP3.Methods.Enumerations.ServerCommands.Failed.ToString());
        }

       
        #endregion

        #region cmsSystem Operations

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Log("Exiting");
            if (tcpServer.Listening)
            {

                tcpServer.Broadcast(RaptorTCP3.Methods.Enumerations.ServerCommands.Wait.ToString()); // Tell all the Clients to wait till I return
                tcpServer.DisconnectAll();
            }


            Application.Exit();
        }
        #endregion

        #region Utilities



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

        #region Global Shared Events

         void ProgressMaximumChangedEvent(int Max)
        {
            if (toolStripContainer1.InvokeRequired)
            {
                SetToolStripProgressMaximumCallBack d = new SetToolStripProgressMaximumCallBack(ProgressMaximumChangedEvent);
                this.Invoke(d, new object[] { Max });
            }
            else
            {
                Progress.Maximum = int.Parse(Max.ToString());
            }
        }

        void  ProgressChangedEvent(int Value)
        {
            if (toolStripContainer1.InvokeRequired)
            {
                SetToolStripProgressValueCallBack d = new SetToolStripProgressValueCallBack(ProgressChangedEvent);
                this.Invoke(d, new object[] { Value });
            }
            else
            {
                Progress.Maximum = int.Parse(Value.ToString());
            }
        }

       void LogEvent(string Message)
        {
            Log(Message);
        }

        #endregion

        #region Form Events

        private void frmMain_Load(object sender, EventArgs e)
        {
            t = new Task(() => StartUp());
            t.Start();
        }

      

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            panel1.Top = 0;
            panel1.Left = 0;
            panel1.Width = this.Width;
            panel1.Height = this.Height / 3;

            ConOut.Left = 0;
            ConOut.Top = this.Height / 3;
            ConOut.Width = this.Width;
            ConOut.Height = (2 * this.Height / 3) - 50;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log("Application Closing");
            Logoff.LogOffAllUsers();
            tcpServer.Broadcast(RaptorTCP3.Methods.Enumerations.ServerCommands.UseCache.ToString());
            t.Dispose();
            t.Wait();
        }
        #endregion

        #region Menu Events
        private void seedUrlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Seeding.SeedUrls();
        }
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fullResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Seeding.SeedUsers();
            Seeding.SeedUrls();
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

        #endregion

        private void timerOneSecond_Tick(object sender, EventArgs e)
        {
            SecondsPastSinceBoot++;

            if (IsIdle)
                SecondsIdle++;
            else
                SecondsIdle = 0;


            UpdateToolStripStatusLabel(lblRuntime, Utils.SecondsToDHMS(SecondsPastSinceBoot).ToString());
            UpdateToolStripStatusLabel(lblIdleTime, Utils.SecondsToDHMS(SecondsIdle).ToString());
        }

        private string LastLogMessage = "";

        /// <summary>
        /// Logs the activities of both the TCP Server AND the SQL sub-system.
        /// </summary>
        /// <param name="message">
        /// String: Any message that needs to be displayed.
        /// </param>
        internal void Log(string message)
        {
            if (LastLogMessage == message) // Only show a log message if it is not an exact repeat of the previous log message.
            { }
            else
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

                }
                LastLogMessage = message;
            }
            UpdateStatusMessage(message);
            Application.DoEvents();
        }



       

    }
}
