namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.importedevidencedatadocuments_st")]
    public partial class importedevidencedatadocuments_st
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdIED { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdEDD { get; set; }

        public bool? Dummy { get; set; }

        public virtual evidencedatadocuments evidencedatadocuments { get; set; }

        public virtual importedevidencedata importedevidencedata { get; set; }
    }
}
