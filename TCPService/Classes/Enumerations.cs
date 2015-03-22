using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaptorTCP3.Methods
{
    class Enumerations
    {

        internal enum ServerCommands
        {
            Successful,
            Failed,
            UseCache,
            Wait,
            Resume,
            SendEmailAddress,
            SetMessageSize,
        }

        internal enum ClientCommands
        {
            Login,
            Register,
            Get,
            NOP,
        }

    }
}
