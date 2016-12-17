namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.evidencedatadocuments")]
    public partial class evidencedatadocuments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public evidencedatadocuments()
        {
            evidencedatadocuments_st = new HashSet<evidencedatadocuments_st>();
            importedevidencedatadocuments_st = new HashSet<importedevidencedatadocuments_st>();
        }

        [Key]
        public int IdEDD { get; set; }

        [Required]
        public byte[] DataEDD { get; set; }

        [Required]
        [StringLength(3)]
        public string TypeEDD { get; set; }

        [StringLength(5)]
        public string FileTypeEDD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<evidencedatadocuments_st> evidencedatadocuments_st { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedevidencedatadocuments_st> importedevidencedatadocuments_st { get; set; }
    }
}
