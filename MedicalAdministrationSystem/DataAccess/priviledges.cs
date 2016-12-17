namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.priviledges")]
    public partial class priviledges
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public priviledges()
        {
            accountdata = new HashSet<accountdata>();
        }

        [Key]
        public int IdP { get; set; }

        [Required]
        [StringLength(45)]
        public string NameP { get; set; }

        public bool ScheduleP { get; set; }

        public bool PatientP { get; set; }

        public bool ExaminationP { get; set; }

        public bool LabP { get; set; }

        public bool EvidenceP { get; set; }

        public bool PrescriptionP { get; set; }

        public bool BillingP { get; set; }

        public bool StatisticP { get; set; }

        public bool UsersP { get; set; }

        public bool SettingP { get; set; }

        public bool HelpP { get; set; }

        public bool LogoutP { get; set; }

        public bool AllSeeP { get; set; }

        public bool IsDoctorP { get; set; }

        public bool IncludeScheduleP { get; set; }

        public bool JustImportDocumentsP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<accountdata> accountdata { get; set; }
    }
}
