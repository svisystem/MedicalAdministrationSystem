namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.scheduleperson_st")]
    public partial class scheduleperson_st
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public scheduleperson_st()
        {
            scheduledata = new HashSet<scheduledata>();
        }

        [Key]
        public int IdSP { get; set; }

        public int? NewPersonIdSP { get; set; }

        public int? ExistedIdSP { get; set; }

        public virtual newperson newperson { get; set; }

        public virtual patientdata patientdata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<scheduledata> scheduledata { get; set; }
    }
}
