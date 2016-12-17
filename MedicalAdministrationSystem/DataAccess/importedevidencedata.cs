namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.importedevidencedata")]
    public partial class importedevidencedata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public importedevidencedata()
        {
            examinationeachimportedevidence_st = new HashSet<examinationeachimportedevidence_st>();
            importedevidencedatadocuments_st = new HashSet<importedevidencedatadocuments_st>();
            importedexaminationeachimportedevidence_st = new HashSet<importedexaminationeachimportedevidence_st>();
        }

        [Key]
        public int IdIED { get; set; }

        public int PatientIED { get; set; }

        public int UserDataIdIED { get; set; }

        public DateTime DateTimeIED { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(15)]
        public string CodeIED { get; set; }

        public int CompanyIdIED { get; set; }

        public virtual companydata companydata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<examinationeachimportedevidence_st> examinationeachimportedevidence_st { get; set; }

        public virtual patientdata patientdata { get; set; }

        public virtual userdata userdata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedevidencedatadocuments_st> importedevidencedatadocuments_st { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedexaminationeachimportedevidence_st> importedexaminationeachimportedevidence_st { get; set; }
    }
}
