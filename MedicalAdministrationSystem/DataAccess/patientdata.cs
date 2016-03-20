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
    
    public partial class patientdata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public patientdata()
        {
            this.belong_st = new HashSet<belong_st>();
            this.evidencedata = new HashSet<evidencedata>();
            this.examinationdata = new HashSet<examinationdata>();
            this.scheduledata = new HashSet<scheduledata>();
        }
    
        public int IdPD { get; set; }
        public string NamePD { get; set; }
        public string BirthNamePD { get; set; }
        public int GenderPD { get; set; }
        public string MotherNamePD { get; set; }
        public int BirthPlacePD { get; set; }
        public System.DateTime BirthDatePD { get; set; }
        public string TAJNumberPD { get; set; }
        public Nullable<long> TAXNumberPD { get; set; }
        public int ZipCodePD { get; set; }
        public int SettlementPD { get; set; }
        public string AddressPD { get; set; }
        public string PhonePD { get; set; }
        public string MobilePhonePD { get; set; }
        public string EmailPD { get; set; }
        public string BillingNamePD { get; set; }
        public int BillingZipCodePD { get; set; }
        public int BillingSettlementPD { get; set; }
        public string BillingAddressPD { get; set; }
        public string NotesPD { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<belong_st> belong_st { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<evidencedata> evidencedata { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<examinationdata> examinationdata { get; set; }
        public virtual gender_fx gender_fx { get; set; }
        public virtual settlement_fx settlement_fx { get; set; }
        public virtual settlement_fx settlement_fx1 { get; set; }
        public virtual settlement_fx settlement_fx2 { get; set; }
        public virtual zipcode_fx zipcode_fx { get; set; }
        public virtual zipcode_fx zipcode_fx1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<scheduledata> scheduledata { get; set; }
    }
}