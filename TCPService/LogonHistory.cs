//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TCPService
{
    using System;
    using System.Collections.Generic;
    
    public partial class LogonHistory
    {
        public int LogonHistoryId { get; set; }
        public int UserId { get; set; }
        public System.DateTime LoggedOnDate { get; set; }
        public Nullable<System.DateTime> LoggedOffDate { get; set; }
    
        public virtual User User { get; set; }
    }
}
