namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.evidencedata")]
    public partial class evidencedata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public evidencedata()
        {
            evidencedatadocuments_st = new HashSet<evidencedatadocuments_st>();
            examinationeachevidence_st = new HashSet<examinationeachevidence_st>();
            importedexaminationeachevidence_st = new HashSet<importedexaminationeachevidence_st>();
        }

        [Key]
        public int IdED { get; set; }

        public int PatientIdED { get; set; }

        public int UserDataIdED { get; set; }

        public DateTime DateTimeED { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(15)]
        public string CodeED { get; set; }

        public int CompanyIdED { get; set; }

        public virtual companydata companydata { get; set; }

        public virtual userdata userdata { get; set; }

        public virtual patientdata patientdata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<evidencedatadocuments_st> evidencedatadocuments_st { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<examinationeachevidence_st> examinationeachevidence_st { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedexaminationeachevidence_st> importedexaminationeachevidence_st { get; set; }
    }
}
