namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.gender_fx")]
    public partial class gender_fx
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public gender_fx()
        {
            patientdata = new HashSet<patientdata>();
            userdata = new HashSet<userdata>();
        }

        [Key]
        public int IdG { get; set; }

        [Required]
        [StringLength(10)]
        public string DataG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<patientdata> patientdata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<userdata> userdata { get; set; }
    }
}
