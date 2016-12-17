namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.scheduledata")]
    public partial class scheduledata
    {
        [Key]
        public int IdSD { get; set; }

        public bool StillNotVisitedSD { get; set; }

        public int PatientIdSD { get; set; }

        public DateTime StartSD { get; set; }

        public DateTime FinishSD { get; set; }

        public int? DoctorIdSD { get; set; }

        [StringLength(1073741823)]
        public string NotesSD { get; set; }

        public int StatusSD { get; set; }

        public virtual userdata userdata { get; set; }

        public virtual scheduleperson_st scheduleperson_st { get; set; }

        public virtual status_fx status_fx { get; set; }
    }
}
