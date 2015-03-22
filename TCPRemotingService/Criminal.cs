//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TCPRemotingService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Criminal
    {
        public Criminal()
        {
            this.ARCs = new HashSet<ARC>();
            this.CrimeTypes = new HashSet<CrimeType>();
        }
    
        public int CId { get; set; }
        public int UId { get; set; }
        public int CrimeId { get; set; }
        public int JurisidictionId { get; set; }
    
        public virtual ICollection<ARC> ARCs { get; set; }
        public virtual ICollection<CrimeType> CrimeTypes { get; set; }
        public virtual CrimeType CrimeType { get; set; }
        public virtual Jurisdiction Jurisdiction { get; set; }
        public virtual User User { get; set; }
    }
}
