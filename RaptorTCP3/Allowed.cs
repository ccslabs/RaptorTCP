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
    
    public partial class Allowed
    {
        public Allowed()
        {
            this.ARCs = new HashSet<ARC>();
        }
    
        public int AId { get; set; }
        public int Uid { get; set; }
        public int JurisidctionId { get; set; }
    
        public virtual Jurisdiction Jurisdiction { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ARC> ARCs { get; set; }
    }
}
