﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DamoclesEntities : DbContext
    {
        public DamoclesEntities()
            : base("name=DamoclesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccountStatu> AccountStatus { get; set; }
        public virtual DbSet<Allowed> Alloweds { get; set; }
        public virtual DbSet<ARC> ARCs { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ContentObject> ContentObjects { get; set; }
        public virtual DbSet<ContentObjectsSourceURL> ContentObjectsSourceURLS { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CrimeType> CrimeTypes { get; set; }
        public virtual DbSet<Criminal> Criminals { get; set; }
        public virtual DbSet<Jurisdiction> Jurisdictions { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LicenseNumber> LicenseNumbers { get; set; }
        public virtual DbSet<LogonHistory> LogonHistories { get; set; }
        public virtual DbSet<Restricted> Restricteds { get; set; }
        public virtual DbSet<RestrictionType> RestrictionTypes { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<URL> URLS { get; set; }
        public virtual DbSet<UrlSource> UrlSources { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<RaptorTCPServerIP> RaptorTCPServerIPs { get; set; }
    }
}
