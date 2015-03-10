using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaptorTCP3.Methods.Utilities
{
    class Utilities
    {
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
