﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TCPService
{

    public class TCPService : ITCPService
    {
        public bool Hello()
        {
            return true;
        }
    }
}
