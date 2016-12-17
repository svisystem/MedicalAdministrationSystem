namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.companydata")]
    public partial class companydata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public companydata()
        {
            billing = new HashSet<billing>();
            billing1 = new HashSet<billing>();
            evidencedata = new HashSet<evidencedata>();
            examinationdata = new HashSet<examinationdata>();
            importedevidencedata = new HashSet<importedevidencedata>();
            importedexaminationdata = new HashSet<importedexaminationdata>();
        }

        [Key]
        public int IdCD { get; set; }

        [Required]
        [StringLength(200)]
        public string NameCD { get; set; }

        public int ZipCodeCD { get; set; }

        public int SettlementCD { get; set; }

        [Required]
        [StringLength(200)]
        public string AddressCD { get; set; }

        [Required]
        [StringLength(17)]
        public string TAXNumberCD { get; set; }

        [StringLength(16)]
        public string RegistrationNumberCD { get; set; }

        [StringLength(30)]
        public string InvoiceNumberCD { get; set; }

        [StringLength(19)]
        public string PhoneCD { get; set; }

        [StringLength(200)]
        public string EmailCD { get; set; }

        [StringLength(200)]
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
