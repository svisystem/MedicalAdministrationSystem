namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.accountdata")]
    public partial class accountdata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public accountdata()
        {
            userdata = new HashSet<userdata>();
        }

        [Key]
        public int IdAD { get; set; }

        public DateTime RegistrateTimeAD { get; set; }

        [Required]
        [StringLength(45)]
        public string UserNameAD { get; set; }

        [Required]
        [StringLength(45)]
        public string PasswordAD { get; set; }

        [Required]
        [StringLength(45)]
        public string PassSaltAD { get; set; }

        public int PriviledgesIdAD { get; set; }

        public bool VerifiedByAdminAD { get; set; }

        public bool DeletedAD { get; set; }

        public DateTime? DeletedTimeAD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<userdata> userdata { get; set; }

        public virtual priviledges priviledges { get; set; }
    }
}
