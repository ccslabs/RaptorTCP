//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RaptorTCP3
{
    using System;
    using System.Collections.Generic;
    
    public partial class CrimeType
    {
        public CrimeType()
        {
            this.Criminals = new HashSet<Criminal>();
        }
    
        public int CrimeId { get; set; }
        public string CrimeName { get; set; }
        public int CId { get; set; }
    
        public virtual Criminal Criminal { get; set; }
        public virtual ICollection<Criminal> Criminals { get; set; }
    }
}
