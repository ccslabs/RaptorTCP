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
    
    public partial class ContentObjectsSourceURL
    {
        public string URLHash { get; set; }
        public int ContentObjectId { get; set; }
        public long Id { get; set; }
    
        public virtual ContentObject ContentObject { get; set; }
    }
}
