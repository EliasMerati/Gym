//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class People
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public People()
        {
            this.Logs = new HashSet<Logs>();
            this.NewProgram = new HashSet<NewProgram>();
            this.Payment = new HashSet<Payment>();
            this.Period = new HashSet<Period>();
            this.Program = new HashSet<Program>();
            this.Shahrieh = new HashSet<Shahrieh>();
        }
    
        public int PeopleID { get; set; }
        public string PeopleName { get; set; }
        public string PeopleMobile { get; set; }
        public string PeopleAddress { get; set; }
        public string PeopleMellicode { get; set; }
        public string PeopleType { get; set; }
        public string PeopleType2 { get; set; }
        public int PeopleDeptor { get; set; }
        public int PeopleCreditor { get; set; }
        public byte[] PeopleLogo { get; set; }
        public System.Guid PicID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Logs> Logs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NewProgram> NewProgram { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Period> Period { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Program> Program { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shahrieh> Shahrieh { get; set; }
    }
}
