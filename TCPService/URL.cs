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
    
    public partial class URL
    {
        public URL()
        {
            this.UrlSources = new HashSet<UrlSource>();
            this.UrlSources1 = new HashSet<UrlSource>();
        }
    
        public int UrlId { get; set; }
        public string UrlHash { get; set; }
        public string URLPath { get; set; }
        public System.DateTime DiscoveryDate { get; set; }
        public int DiscoveredById { get; set; }
        public Nullable<System.DateTime> ProcessedDate { get; set; }
        public Nullable<int> ProcessedById { get; set; }
        public bool IsInProcessingQueue { get; set; }
        public Nullable<System.DateTime> JoinedProcessingQueueDate { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual ICollection<UrlSource> UrlSources { get; set; }
        public virtual ICollection<UrlSource> UrlSources1 { get; set; }
    }
}
