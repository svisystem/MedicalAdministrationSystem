namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.importedexaminationeachevidence_st")]
    public partial class importedexaminationeachevidence_st
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdIEX { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdED { get; set; }

        public bool? Dummy { get; set; }

        public virtual evidencedata evidencedata { get; set; }

        public virtual importedexaminationdata importedexaminationdata { get; set; }
    }
}
