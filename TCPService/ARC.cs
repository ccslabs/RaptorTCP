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
    
    public partial class ARC
    {
        public ARC()
        {
            this.ContentObjects = new HashSet<ContentObject>();
        }
    
        public int ARCId { get; set; }
        public Nullable<int> AId { get; set; }
        public Nullable<int> RId { get; set; }
        public Nullable<int> CId { get; set; }
    
        public virtual Allowed Allowed { get; set; }
        public virtual Criminal Criminal { get; set; }
        public virtual Restricted Restricted { get; set; }
        public virtual ICollection<ContentObject> ContentObjects { get; set; }
    }
}
