namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.exceptedschedule")]
    public partial class exceptedschedule
    {
        [Key]
        public int IdES { get; set; }

        public bool IncludedES { get; set; }

        public int UserDataIdES { get; set; }

        public DateTime StartDateES { get; set; }

        public DateTime FinishDateED { get; set; }

        public virtual userdata userdata { get; set; }
    }
}
