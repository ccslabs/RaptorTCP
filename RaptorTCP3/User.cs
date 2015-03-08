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
    
    public partial class User
    {
        public User()
        {
            this.Alloweds = new HashSet<Allowed>();
            this.Criminals = new HashSet<Criminal>();
            this.LogonHistories = new HashSet<LogonHistory>();
            this.News = new HashSet<News>();
            this.Restricteds = new HashSet<Restricted>();
            this.URLS = new HashSet<URL>();
            this.URLS1 = new HashSet<URL>();
        }
    
        public int UserId { get; set; }
        public string Username { get; set; }
        public string UserPasswordHash { get; set; }
        public System.DateTime RegisteredDate { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int JurisidictionId { get; set; }
        public int LanguagesId { get; set; }
        public bool IsOnline { get; set; }
        public int AccountStatusId { get; set; }
        public string LicenseNumber { get; set; }
        public string emailAddress { get; set; }
    
        public virtual ICollection<Allowed> Alloweds { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<Criminal> Criminals { get; set; }
        public virtual Jurisdiction Jurisdiction { get; set; }
        public virtual Language Language { get; set; }
        public virtual ICollection<LogonHistory> LogonHistories { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Restricted> Restricteds { get; set; }
        public virtual State State { get; set; }
        public virtual ICollection<URL> URLS { get; set; }
        public virtual ICollection<URL> URLS1 { get; set; }
    }
}
