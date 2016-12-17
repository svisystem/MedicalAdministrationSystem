namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.servicesdata")]
    public partial class servicesdata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public servicesdata()
        {
            examinationdata = new HashSet<examinationdata>();
            pricesforeachservice = new HashSet<pricesforeachservice>();
        }

        [Key]
        public int IdTD { get; set; }

        [Required]
        [StringLength(200)]
        public string NameTD { get; set; }

        [StringLength(1073741823)]
        public string DetailsTD { get; set; }

        public DateTime? DeletedTD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<examinationdata> examinationdata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pricesforeachservice> pricesforeachservice { get; set; }
    }
}
