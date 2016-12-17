namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.pricesforeachservice")]
    public partial class pricesforeachservice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pricesforeachservice()
        {
            currentpricesforeachbill_st = new HashSet<currentpricesforeachbill_st>();
        }

        [Key]
        public int IdPFS { get; set; }

        public int VatPFS { get; set; }

        public int PricePFS { get; set; }

        public int ServiceDataIdPFS { get; set; }

        public DateTime WhenChangedPFS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<currentpricesforeachbill_st> currentpricesforeachbill_st { get; set; }

        public virtual servicesdata servicesdata { get; set; }
    }
}
