namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.usersschedule")]
    public partial class usersschedule
    {
        [Key]
        public int IdUS { get; set; }

        public int UserDataIdUS { get; set; }

        public int DayOfWeekUS { get; set; }

        public DateTime StartTimeUS { get; set; }

        public DateTime FinishTimeUS { get; set; }

        public DateTime WhenCreateUS { get; set; }

        public virtual userdata userdata { get; set; }
    }
}
