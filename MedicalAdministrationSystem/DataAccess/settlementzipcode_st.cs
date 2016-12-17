namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.settlementzipcode_st")]
    public partial class settlementzipcode_st
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdZC { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdS { get; set; }

        public bool? Dummy { get; set; }

        public virtual settlement_fx settlement_fx { get; set; }

        public virtual zipcode_fx zipcode_fx { get; set; }
    }
}
