namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.examinationdata")]
    public partial class examinationdata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public examinationdata()
        {
            examinationdatadocuments_st = new HashSet<examinationdatadocuments_st>();
            examinationeachevidence_st = new HashSet<examinationeachevidence_st>();
            examinationeachimportedevidence_st = new HashSet<examinationeachimportedevidence_st>();
        }

        [Key]
        public int IdEX { get; set; }

        public int PatientIdEX { get; set; }

        public int DoctorIdEX { get; set; }

        public DateTime DateTimeEX { get; set; }

        public int ServiceIdEX { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(15)]
        public string CodeEX { get; set; }

        public int CompanyIdEX { get; set; }

        public virtual companydata companydata { get; set; }

        public virtual patientdata patientdata { get; set; }

        public virtual servicesdata servicesdata { get; set; }

        public virtual userdata userdata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<examinationdatadocuments_st> examinationdatadocuments_st { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<examinationeachevidence_st> examinationeachevidence_st { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<examinationeachimportedevidence_st> examinationeachimportedevidence_st { get; set; }
    }
}
