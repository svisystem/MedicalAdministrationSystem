namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.importedexaminationdata")]
    public partial class importedexaminationdata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public importedexaminationdata()
        {
            importedexaminationdatadocuments_st = new HashSet<importedexaminationdatadocuments_st>();
            importedexaminationeachevidence_st = new HashSet<importedexaminationeachevidence_st>();
            importedexaminationeachimportedevidence_st = new HashSet<importedexaminationeachimportedevidence_st>();
        }

        [Key]
        public int IdIEX { get; set; }

        public int PatientIdIEX { get; set; }

        public int DoctorIdIEX { get; set; }

        public DateTime DateTimeIEX { get; set; }

        [Required]
        [StringLength(200)]
        public string NameIEX { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(15)]
        public string CodeIEX { get; set; }

        public int CompanyIdIEX { get; set; }

        public virtual companydata companydata { get; set; }

        public virtual patientdata patientdata { get; set; }

        public virtual userdata userdata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedexaminationdatadocuments_st> importedexaminationdatadocuments_st { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedexaminationeachevidence_st> importedexaminationeachevidence_st { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedexaminationeachimportedevidence_st> importedexaminationeachimportedevidence_st { get; set; }
    }
}
