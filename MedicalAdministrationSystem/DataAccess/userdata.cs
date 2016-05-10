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
    
    public partial class userdata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public userdata()
        {
            this.belong_st = new HashSet<belong_st>();
            this.billing = new HashSet<billing>();
            this.evidencedata = new HashSet<evidencedata>();
            this.examinationdata = new HashSet<examinationdata>();
            this.exceptedschedule = new HashSet<exceptedschedule>();
            this.importedevidencedata = new HashSet<importedevidencedata>();
            this.importedexaminationdata = new HashSet<importedexaminationdata>();
            this.scheduledata = new HashSet<scheduledata>();
            this.usersschedule = new HashSet<usersschedule>();
        }
    
        public int IdUD { get; set; }
        public int AccountDataIdUD { get; set; }
        public string NameUD { get; set; }
        public string BirthNameUD { get; set; }
        public string JobTitleUD { get; set; }
        public Nullable<int> SealNumberUD { get; set; }
        public string TAJNumberUD { get; set; }
        public Nullable<long> TAXNumberUD { get; set; }
        public int GenderUD { get; set; }
        public string MotherNameUD { get; set; }
        public System.DateTime BirthDateUD { get; set; }
        public int BirthPlaceUD { get; set; }
        public Nullable<int> ZipCodeUD { get; set; }
        public Nullable<int> SettlementUD { get; set; }
        public string AddressUD { get; set; }
        public string PhoneUD { get; set; }
        public string JobPhoneUD { get; set; }
        public string EmailUD { get; set; }
    
        public virtual accountdata accountdata { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<belong_st> belong_st { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<billing> billing { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<evidencedata> evidencedata { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<examinationdata> examinationdata { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<exceptedschedule> exceptedschedule { get; set; }
        public virtual gender_fx gender_fx { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedevidencedata> importedevidencedata { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedexaminationdata> importedexaminationdata { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<scheduledata> scheduledata { get; set; }
        public virtual settlement_fx settlement_fx { get; set; }
        public virtual settlement_fx settlement_fx1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<usersschedule> usersschedule { get; set; }
        public virtual zipcode_fx zipcode_fx { get; set; }
    }
}
