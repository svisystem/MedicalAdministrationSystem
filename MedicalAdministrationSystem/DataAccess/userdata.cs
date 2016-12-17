namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("medical.userdata")]
    public partial class userdata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public userdata()
        {
            belong_st = new HashSet<belong_st>();
            billing = new HashSet<billing>();
            evidencedata = new HashSet<evidencedata>();
            examinationdata = new HashSet<examinationdata>();
            exceptedschedule = new HashSet<exceptedschedule>();
            importedevidencedata = new HashSet<importedevidencedata>();
            importedexaminationdata = new HashSet<importedexaminationdata>();
            scheduledata = new HashSet<scheduledata>();
            usersschedule = new HashSet<usersschedule>();
        }

        [Key]
        public int IdUD { get; set; }

        public int AccountDataIdUD { get; set; }

        [Required]
        [StringLength(200)]
        public string NameUD { get; set; }

        [StringLength(200)]
        public string BirthNameUD { get; set; }

        [Required]
        [StringLength(100)]
        public string JobTitleUD { get; set; }

        public int? SealNumberUD { get; set; }

        [Required]
        [StringLength(15)]
        public string TAJNumberUD { get; set; }

        public long? TAXNumberUD { get; set; }

        public int GenderUD { get; set; }

        [StringLength(200)]
        public string MotherNameUD { get; set; }

        [Column(TypeName = "date")]
        public DateTime BirthDateUD { get; set; }

        public int BirthPlaceUD { get; set; }

        public int? ZipCodeUD { get; set; }

        public int? SettlementUD { get; set; }

        [StringLength(200)]
        public string AddressUD { get; set; }

        [StringLength(18)]
        public string PhoneUD { get; set; }

        [StringLength(19)]
        public string JobPhoneUD { get; set; }

        [StringLength(200)]
        public string EmailUD { get; set; }

        public virtual accountdata accountdata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<belong_st> belong_st { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<billing> billing { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<evidencedata> evidencedata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<examinationdata> examinationdata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<exceptedschedule> exceptedschedule { get; set; }

        public virtual gender_fx gender_fx { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedevidencedata> importedevidencedata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<importedexaminationdata> importedexaminationdata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<scheduledata> scheduledata { get; set; }

        public virtual settlement_fx settlement_fx { get; set; }

        public virtual settlement_fx settlement_fx1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<usersschedule> usersschedule { get; set; }

        public virtual zipcode_fx zipcode_fx { get; set; }
    }
}
