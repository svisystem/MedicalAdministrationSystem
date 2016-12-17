namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.importedexaminationdatadocuments_st")]
    public partial class importedexaminationdatadocuments_st
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdIEX { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdEXD { get; set; }

        public bool? Dummy { get; set; }

        public virtual examinationdatadocuments examinationdatadocuments { get; set; }

        public virtual importedexaminationdata importedexaminationdata { get; set; }
    }
}
