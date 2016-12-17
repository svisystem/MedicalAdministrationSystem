namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.importedexaminationeachimportedevidence_st")]
    public partial class importedexaminationeachimportedevidence_st
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdIEX { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdIED { get; set; }

        public bool? Dummy { get; set; }

        public virtual importedevidencedata importedevidencedata { get; set; }

        public virtual importedexaminationdata importedexaminationdata { get; set; }
    }
}
