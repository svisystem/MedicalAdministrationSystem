//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class companydata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public companydata()
        {
            this.billing = new HashSet<billing>();
            this.billing1 = new HashSet<billing>();
            this.evidencedata = new HashSet<evidencedata>();
            this.examinationdata = new HashSet<examinationdata>();
            this.importedevidencedata = new HashSet<importedevidencedata>();
            this.importedexaminationdata = new HashSet<importedexaminationdata>();
        }
    
        public int IdCD { get; set; }
        public string NameCD { get; set; }
        public int ZipCodeCD { get; set; }
        public int SettlementCD { get; set; }
        public string AddressCD { get; set; }
        public string TAXNumberCD { get; set; }
        public string RegistrationNumberCD { get; set; }
        public string InvoiceNumberCD { get; set; }
        public string PhoneCD { get; set; }
        public string EmailCD { get; set; }
        public string WebPageCD { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<billing> billing { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<billing> billing1 { get; set; }
        public virtual settlement_fx settlement_fx { get; set; }
        public virtual zipcode_fx zipcode_fx { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<evidencedata> evidencedata { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<examinationdata> examinationdata { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedevidencedata> importedevidencedata { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedexaminationdata> importedexaminationdata { get; set; }
    }
}
