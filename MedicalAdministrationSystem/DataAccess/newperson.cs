namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.newperson")]
    public partial class newperson
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public newperson()
        {
            scheduleperson_st = new HashSet<scheduleperson_st>();
        }

        [Key]
        public int IdNP { get; set; }

        [Required]
        [StringLength(200)]
        public string PatientNameNP { get; set; }

        [Required]
        [StringLength(15)]
        public string TAJNumberNP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<scheduleperson_st> scheduleperson_st { get; set; }
    }
}
