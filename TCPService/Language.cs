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
    
    public partial class Language
    {
        public Language()
        {
            this.Jurisdictions = new HashSet<Jurisdiction>();
            this.Users = new HashSet<User>();
        }
    
        public int LanguageId { get; set; }
        public string LanguageEnglishName { get; set; }
        public string LanguageLocalName { get; set; }
        public bool TranslationAvailable { get; set; }
    
        public virtual ICollection<Jurisdiction> Jurisdictions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
