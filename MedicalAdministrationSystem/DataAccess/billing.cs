namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.billing")]
    public partial class billing
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public billing()
        {
            currentpricesforeachbill_st = new HashSet<currentpricesforeachbill_st>();
        }

        [Key]
        public int IdB { get; set; }

        public int PatientIdB { get; set; }

        public int UserIdB { get; set; }

        public int CompanyIdFromB { get; set; }

        public int? CompanyIdToB { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(15)]
        public string CodeB { get; set; }

        public DateTime DateTimeB { get; set; }

        [Required]
        public byte[] BillB { get; set; }

        public virtual companydata companydata { get; set; }

        public virtual companydata companydata1 { get; set; }

        public virtual patientdata patientdata { get; set; }

        public virtual userdata userdata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<currentpricesforeachbill_st> currentpricesforeachbill_st { get; set; }
    }
}
