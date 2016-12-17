namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.currentpricesforeachbill_st")]
    public partial class currentpricesforeachbill_st
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdB { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPFS { get; set; }

        public bool? Dummy { get; set; }

        public virtual billing billing { get; set; }

        public virtual pricesforeachservice pricesforeachservice { get; set; }
    }
}
