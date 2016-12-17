namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.patientdata")]
    public partial class patientdata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public patientdata()
        {
            belong_st = new HashSet<belong_st>();
            billing = new HashSet<billing>();
            evidencedata = new HashSet<evidencedata>();
            examinationdata = new HashSet<examinationdata>();
            importedevidencedata = new HashSet<importedevidencedata>();
            importedexaminationdata = new HashSet<importedexaminationdata>();
            scheduleperson_st = new HashSet<scheduleperson_st>();
        }

        [Key]
        public int IdPD { get; set; }

        [Required]
        [StringLength(200)]
        public string NamePD { get; set; }

        [StringLength(200)]
        public string BirthNamePD { get; set; }

        public int GenderPD { get; set; }

        [Required]
        [StringLength(200)]
        public string MotherNamePD { get; set; }

        public int BirthPlacePD { get; set; }

        [Column(TypeName = "date")]
        public DateTime BirthDatePD { get; set; }

        [Required]
        [StringLength(15)]
        public string TAJNumberPD { get; set; }

        public long? TAXNumberPD { get; set; }

        public int ZipCodePD { get; set; }

        public int SettlementPD { get; set; }

        [Required]
        [StringLength(200)]
        public string AddressPD { get; set; }

        [StringLength(19)]
        public string PhonePD { get; set; }

        [StringLength(19)]
        public string MobilePhonePD { get; set; }

        [StringLength(200)]
        public string EmailPD { get; set; }

        [Required]
        [StringLength(200)]
        public string BillingNamePD { get; set; }

        public int BillingZipCodePD { get; set; }

        public int BillingSettlementPD { get; set; }

        [Required]
        [StringLength(200)]
        public string BillingAddressPD { get; set; }

        public DateTime CreatedPD { get; set; }

        [StringLength(1073741823)]
        public string NotesPD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<belong_st> belong_st { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<billing> billing { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<evidencedata> evidencedata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<examinationdata> examinationdata { get; set; }

        public virtual gender_fx gender_fx { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedevidencedata> importedevidencedata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedexaminationdata> importedexaminationdata { get; set; }

        public virtual settlement_fx settlement_fx { get; set; }

        public virtual settlement_fx settlement_fx1 { get; set; }

        public virtual settlement_fx settlement_fx2 { get; set; }

        public virtual zipcode_fx zipcode_fx { get; set; }

        public virtual zipcode_fx zipcode_fx1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<scheduleperson_st> scheduleperson_st { get; set; }
    }
}
