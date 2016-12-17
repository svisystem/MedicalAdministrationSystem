namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.examinationdatadocuments")]
    public partial class examinationdatadocuments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public examinationdatadocuments()
        {
            examinationdatadocuments_st = new HashSet<examinationdatadocuments_st>();
            importedexaminationdatadocuments_st = new HashSet<importedexaminationdatadocuments_st>();
        }

        [Key]
        public int IdEXD { get; set; }

        [Required]
        public byte[] DataEXD { get; set; }

        [Required]
        [StringLength(3)]
        public string TypeEXD { get; set; }

        [StringLength(5)]
        public string FileTypeEXD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<examinationdatadocuments_st> examinationdatadocuments_st { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedexaminationdatadocuments_st> importedexaminationdatadocuments_st { get; set; }
    }
}
