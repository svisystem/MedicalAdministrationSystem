namespace MedicalAdministrationSystem.DataAccess
{
    using System.Data.Entity;

    public partial class MedicalModel : DbContext
    {
        public MedicalModel(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public virtual DbSet<accountdata> accountdata { get; set; }
        public virtual DbSet<belong_st> belong_st { get; set; }
        public virtual DbSet<billing> billing { get; set; }
        public virtual DbSet<companydata> companydata { get; set; }
        public virtual DbSet<currentpricesforeachbill_st> currentpricesforeachbill_st { get; set; }
        public virtual DbSet<evidencedata> evidencedata { get; set; }
        public virtual DbSet<evidencedatadocuments> evidencedatadocuments { get; set; }
        public virtual DbSet<evidencedatadocuments_st> evidencedatadocuments_st { get; set; }
        public virtual DbSet<examinationdata> examinationdata { get; set; }
        public virtual DbSet<examinationdatadocuments> examinationdatadocuments { get; set; }
        public virtual DbSet<examinationdatadocuments_st> examinationdatadocuments_st { get; set; }
        public virtual DbSet<examinationeachevidence_st> examinationeachevidence_st { get; set; }
        public virtual DbSet<examinationeachimportedevidence_st> examinationeachimportedevidence_st { get; set; }
        public virtual DbSet<exceptedschedule> exceptedschedule { get; set; }
        public virtual DbSet<gender_fx> gender_fx { get; set; }
        public virtual DbSet<importedevidencedata> importedevidencedata { get; set; }
        public virtual DbSet<importedevidencedatadocuments_st> importedevidencedatadocuments_st { get; set; }
        public virtual DbSet<importedexaminationdata> importedexaminationdata { get; set; }
        public virtual DbSet<importedexaminationdatadocuments_st> importedexaminationdatadocuments_st { get; set; }
        public virtual DbSet<importedexaminationeachevidence_st> importedexaminationeachevidence_st { get; set; }
        public virtual DbSet<importedexaminationeachimportedevidence_st> importedexaminationeachimportedevidence_st { get; set; }
        public virtual DbSet<newperson> newperson { get; set; }
        public virtual DbSet<patientdata> patientdata { get; set; }
        public virtual DbSet<pricesforeachservice> pricesforeachservice { get; set; }
        public virtual DbSet<priviledges> priviledges { get; set; }
        public virtual DbSet<scheduledata> scheduledata { get; set; }
        public virtual DbSet<scheduleperson_st> scheduleperson_st { get; set; }
        public virtual DbSet<servicesdata> servicesdata { get; set; }
        public virtual DbSet<settlement_fx> settlement_fx { get; set; }
        public virtual DbSet<settlementzipcode_st> settlementzipcode_st { get; set; }
        public virtual DbSet<status_fx> status_fx { get; set; }
        public virtual DbSet<userdata> userdata { get; set; }
        public virtual DbSet<usersschedule> usersschedule { get; set; }
        public virtual DbSet<zipcode_fx> zipcode_fx { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<accountdata>()
                .Property(e => e.UserNameAD)
                .IsUnicode(false);

            modelBuilder.Entity<accountdata>()
                .Property(e => e.PasswordAD)
                .IsUnicode(false);

            modelBuilder.Entity<accountdata>()
                .Property(e => e.PassSaltAD)
                .IsUnicode(false);

            modelBuilder.Entity<accountdata>()
                .HasMany(e => e.userdata)
                .WithRequired(e => e.accountdata)
                .HasForeignKey(e => e.AccountDataIdUD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<billing>()
                .Property(e => e.CodeB)
                .IsUnicode(false);

            modelBuilder.Entity<billing>()
                .HasMany(e => e.currentpricesforeachbill_st)
                .WithRequired(e => e.billing)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<companydata>()
                .Property(e => e.NameCD)
                .IsUnicode(false);

            modelBuilder.Entity<companydata>()
                .Property(e => e.AddressCD)
                .IsUnicode(false);

            modelBuilder.Entity<companydata>()
                .Property(e => e.TAXNumberCD)
                .IsUnicode(false);

            modelBuilder.Entity<companydata>()
                .Property(e => e.RegistrationNumberCD)
                .IsUnicode(false);

            modelBuilder.Entity<companydata>()
                .Property(e => e.InvoiceNumberCD)
                .IsUnicode(false);

            modelBuilder.Entity<companydata>()
                .Property(e => e.PhoneCD)
                .IsUnicode(false);

            modelBuilder.Entity<companydata>()
                .Property(e => e.EmailCD)
                .IsUnicode(false);

            modelBuilder.Entity<companydata>()
                .Property(e => e.WebPageCD)
                .IsUnicode(false);

            modelBuilder.Entity<companydata>()
                .HasMany(e => e.billing)
                .WithRequired(e => e.companydata)
                .HasForeignKey(e => e.CompanyIdFromB)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<companydata>()
                .HasMany(e => e.billing1)
                .WithOptional(e => e.companydata1)
                .HasForeignKey(e => e.CompanyIdToB);

            modelBuilder.Entity<companydata>()
                .HasMany(e => e.evidencedata)
                .WithRequired(e => e.companydata)
                .HasForeignKey(e => e.CompanyIdED)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<companydata>()
                .HasMany(e => e.examinationdata)
                .WithRequired(e => e.companydata)
                .HasForeignKey(e => e.CompanyIdEX)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<companydata>()
                .HasMany(e => e.importedevidencedata)
                .WithRequired(e => e.companydata)
                .HasForeignKey(e => e.CompanyIdIED)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<companydata>()
                .HasMany(e => e.importedexaminationdata)
                .WithRequired(e => e.companydata)
                .HasForeignKey(e => e.CompanyIdIEX)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<evidencedata>()
                .Property(e => e.CodeED)
                .IsUnicode(false);

            modelBuilder.Entity<evidencedata>()
                .HasMany(e => e.evidencedatadocuments_st)
                .WithRequired(e => e.evidencedata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<evidencedata>()
                .HasMany(e => e.examinationeachevidence_st)
                .WithRequired(e => e.evidencedata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<evidencedata>()
                .HasMany(e => e.importedexaminationeachevidence_st)
                .WithRequired(e => e.evidencedata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<evidencedatadocuments>()
                .Property(e => e.TypeEDD)
                .IsUnicode(false);

            modelBuilder.Entity<evidencedatadocuments>()
                .Property(e => e.FileTypeEDD)
                .IsUnicode(false);

            modelBuilder.Entity<evidencedatadocuments>()
                .HasMany(e => e.evidencedatadocuments_st)
                .WithRequired(e => e.evidencedatadocuments)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<evidencedatadocuments>()
                .HasMany(e => e.importedevidencedatadocuments_st)
                .WithRequired(e => e.evidencedatadocuments)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<examinationdata>()
                .Property(e => e.CodeEX)
                .IsUnicode(false);

            modelBuilder.Entity<examinationdata>()
                .HasMany(e => e.examinationdatadocuments_st)
                .WithRequired(e => e.examinationdata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<examinationdata>()
                .HasMany(e => e.examinationeachevidence_st)
                .WithRequired(e => e.examinationdata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<examinationdata>()
                .HasMany(e => e.examinationeachimportedevidence_st)
                .WithRequired(e => e.examinationdata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<examinationdatadocuments>()
                .Property(e => e.TypeEXD)
                .IsUnicode(false);

            modelBuilder.Entity<examinationdatadocuments>()
                .Property(e => e.FileTypeEXD)
                .IsUnicode(false);

            modelBuilder.Entity<examinationdatadocuments>()
                .HasMany(e => e.examinationdatadocuments_st)
                .WithRequired(e => e.examinationdatadocuments)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<examinationdatadocuments>()
                .HasMany(e => e.importedexaminationdatadocuments_st)
                .WithRequired(e => e.examinationdatadocuments)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<gender_fx>()
                .Property(e => e.DataG)
                .IsUnicode(false);

            modelBuilder.Entity<gender_fx>()
                .HasMany(e => e.patientdata)
                .WithRequired(e => e.gender_fx)
                .HasForeignKey(e => e.GenderPD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<gender_fx>()
                .HasMany(e => e.userdata)
                .WithRequired(e => e.gender_fx)
                .HasForeignKey(e => e.GenderUD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<importedevidencedata>()
                .Property(e => e.CodeIED)
                .IsUnicode(false);

            modelBuilder.Entity<importedevidencedata>()
                .HasMany(e => e.examinationeachimportedevidence_st)
                .WithRequired(e => e.importedevidencedata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<importedevidencedata>()
                .HasMany(e => e.importedevidencedatadocuments_st)
                .WithRequired(e => e.importedevidencedata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<importedevidencedata>()
                .HasMany(e => e.importedexaminationeachimportedevidence_st)
                .WithRequired(e => e.importedevidencedata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<importedexaminationdata>()
                .Property(e => e.NameIEX)
                .IsUnicode(false);

            modelBuilder.Entity<importedexaminationdata>()
                .Property(e => e.CodeIEX)
                .IsUnicode(false);

            modelBuilder.Entity<importedexaminationdata>()
                .HasMany(e => e.importedexaminationdatadocuments_st)
                .WithRequired(e => e.importedexaminationdata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<importedexaminationdata>()
                .HasMany(e => e.importedexaminationeachevidence_st)
                .WithRequired(e => e.importedexaminationdata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<importedexaminationdata>()
                .HasMany(e => e.importedexaminationeachimportedevidence_st)
                .WithRequired(e => e.importedexaminationdata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<newperson>()
                .Property(e => e.PatientNameNP)
                .IsUnicode(false);

            modelBuilder.Entity<newperson>()
                .Property(e => e.TAJNumberNP)
                .IsUnicode(false);

            modelBuilder.Entity<newperson>()
                .HasMany(e => e.scheduleperson_st)
                .WithOptional(e => e.newperson)
                .HasForeignKey(e => e.NewPersonIdSP);

            modelBuilder.Entity<patientdata>()
                .Property(e => e.NamePD)
                .IsUnicode(false);

            modelBuilder.Entity<patientdata>()
                .Property(e => e.BirthNamePD)
                .IsUnicode(false);

            modelBuilder.Entity<patientdata>()
                .Property(e => e.MotherNamePD)
                .IsUnicode(false);

            modelBuilder.Entity<patientdata>()
                .Property(e => e.TAJNumberPD)
                .IsUnicode(false);

            modelBuilder.Entity<patientdata>()
                .Property(e => e.AddressPD)
                .IsUnicode(false);

            modelBuilder.Entity<patientdata>()
                .Property(e => e.PhonePD)
                .IsUnicode(false);

            modelBuilder.Entity<patientdata>()
                .Property(e => e.MobilePhonePD)
                .IsUnicode(false);

            modelBuilder.Entity<patientdata>()
                .Property(e => e.EmailPD)
                .IsUnicode(false);

            modelBuilder.Entity<patientdata>()
                .Property(e => e.BillingNamePD)
                .IsUnicode(false);

            modelBuilder.Entity<patientdata>()
                .Property(e => e.BillingAddressPD)
                .IsUnicode(false);

            modelBuilder.Entity<patientdata>()
                .Property(e => e.NotesPD)
                .IsUnicode(false);

            modelBuilder.Entity<patientdata>()
                .HasMany(e => e.belong_st)
                .WithRequired(e => e.patientdata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<patientdata>()
                .HasMany(e => e.billing)
                .WithRequired(e => e.patientdata)
                .HasForeignKey(e => e.PatientIdB)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<patientdata>()
                .HasMany(e => e.evidencedata)
                .WithRequired(e => e.patientdata)
                .HasForeignKey(e => e.PatientIdED)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<patientdata>()
                .HasMany(e => e.examinationdata)
                .WithRequired(e => e.patientdata)
                .HasForeignKey(e => e.PatientIdEX)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<patientdata>()
                .HasMany(e => e.importedevidencedata)
                .WithRequired(e => e.patientdata)
                .HasForeignKey(e => e.PatientIED)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<patientdata>()
                .HasMany(e => e.importedexaminationdata)
                .WithRequired(e => e.patientdata)
                .HasForeignKey(e => e.PatientIdIEX)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<patientdata>()
                .HasMany(e => e.scheduleperson_st)
                .WithOptional(e => e.patientdata)
                .HasForeignKey(e => e.ExistedIdSP);

            modelBuilder.Entity<pricesforeachservice>()
                .HasMany(e => e.currentpricesforeachbill_st)
                .WithRequired(e => e.pricesforeachservice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<priviledges>()
                .Property(e => e.NameP)
                .IsUnicode(false);

            modelBuilder.Entity<priviledges>()
                .HasMany(e => e.accountdata)
                .WithRequired(e => e.priviledges)
                .HasForeignKey(e => e.PriviledgesIdAD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<scheduledata>()
                .Property(e => e.NotesSD)
                .IsUnicode(false);

            modelBuilder.Entity<scheduleperson_st>()
                .HasMany(e => e.scheduledata)
                .WithRequired(e => e.scheduleperson_st)
                .HasForeignKey(e => e.PatientIdSD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<servicesdata>()
                .Property(e => e.NameTD)
                .IsUnicode(false);

            modelBuilder.Entity<servicesdata>()
                .Property(e => e.DetailsTD)
                .IsUnicode(false);

            modelBuilder.Entity<servicesdata>()
                .HasMany(e => e.examinationdata)
                .WithRequired(e => e.servicesdata)
                .HasForeignKey(e => e.ServiceIdEX)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<servicesdata>()
                .HasMany(e => e.pricesforeachservice)
                .WithRequired(e => e.servicesdata)
                .HasForeignKey(e => e.ServiceDataIdPFS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<settlement_fx>()
                .Property(e => e.DataS)
                .IsUnicode(false);

            modelBuilder.Entity<settlement_fx>()
                .HasMany(e => e.companydata)
                .WithRequired(e => e.settlement_fx)
                .HasForeignKey(e => e.SettlementCD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<settlement_fx>()
                .HasMany(e => e.patientdata)
                .WithRequired(e => e.settlement_fx)
                .HasForeignKey(e => e.SettlementPD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<settlement_fx>()
                .HasMany(e => e.patientdata1)
                .WithRequired(e => e.settlement_fx1)
                .HasForeignKey(e => e.BirthPlacePD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<settlement_fx>()
                .HasMany(e => e.patientdata2)
                .WithRequired(e => e.settlement_fx2)
                .HasForeignKey(e => e.BillingSettlementPD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<settlement_fx>()
                .HasMany(e => e.settlementzipcode_st)
                .WithRequired(e => e.settlement_fx)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<settlement_fx>()
                .HasMany(e => e.userdata)
                .WithRequired(e => e.settlement_fx)
                .HasForeignKey(e => e.BirthPlaceUD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<settlement_fx>()
                .HasMany(e => e.userdata1)
                .WithOptional(e => e.settlement_fx1)
                .HasForeignKey(e => e.SettlementUD);

            modelBuilder.Entity<status_fx>()
                .Property(e => e.DataS)
                .IsUnicode(false);

            modelBuilder.Entity<status_fx>()
                .HasMany(e => e.scheduledata)
                .WithRequired(e => e.status_fx)
                .HasForeignKey(e => e.StatusSD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<userdata>()
                .Property(e => e.NameUD)
                .IsUnicode(false);

            modelBuilder.Entity<userdata>()
                .Property(e => e.BirthNameUD)
                .IsUnicode(false);

            modelBuilder.Entity<userdata>()
                .Property(e => e.JobTitleUD)
                .IsUnicode(false);

            modelBuilder.Entity<userdata>()
                .Property(e => e.TAJNumberUD)
                .IsUnicode(false);

            modelBuilder.Entity<userdata>()
                .Property(e => e.MotherNameUD)
                .IsUnicode(false);

            modelBuilder.Entity<userdata>()
                .Property(e => e.AddressUD)
                .IsUnicode(false);

            modelBuilder.Entity<userdata>()
                .Property(e => e.PhoneUD)
                .IsUnicode(false);

            modelBuilder.Entity<userdata>()
                .Property(e => e.JobPhoneUD)
                .IsUnicode(false);

            modelBuilder.Entity<userdata>()
                .Property(e => e.EmailUD)
                .IsUnicode(false);

            modelBuilder.Entity<userdata>()
                .HasMany(e => e.belong_st)
                .WithRequired(e => e.userdata)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<userdata>()
                .HasMany(e => e.billing)
                .WithRequired(e => e.userdata)
                .HasForeignKey(e => e.UserIdB)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<userdata>()
                .HasMany(e => e.evidencedata)
                .WithRequired(e => e.userdata)
                .HasForeignKey(e => e.UserDataIdED)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<userdata>()
                .HasMany(e => e.examinationdata)
                .WithRequired(e => e.userdata)
                .HasForeignKey(e => e.DoctorIdEX)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<userdata>()
                .HasMany(e => e.exceptedschedule)
                .WithRequired(e => e.userdata)
                .HasForeignKey(e => e.UserDataIdES)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<userdata>()
                .HasMany(e => e.importedevidencedata)
                .WithRequired(e => e.userdata)
                .HasForeignKey(e => e.UserDataIdIED)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<userdata>()
                .HasMany(e => e.importedexaminationdata)
                .WithRequired(e => e.userdata)
                .HasForeignKey(e => e.DoctorIdIEX)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<userdata>()
                .HasMany(e => e.scheduledata)
                .WithOptional(e => e.userdata)
                .HasForeignKey(e => e.DoctorIdSD);

            modelBuilder.Entity<userdata>()
                .HasMany(e => e.usersschedule)
                .WithRequired(e => e.userdata)
                .HasForeignKey(e => e.UserDataIdUS)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<zipcode_fx>()
                .HasMany(e => e.companydata)
                .WithRequired(e => e.zipcode_fx)
                .HasForeignKey(e => e.ZipCodeCD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<zipcode_fx>()
                .HasMany(e => e.patientdata)
                .WithRequired(e => e.zipcode_fx)
                .HasForeignKey(e => e.ZipCodePD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<zipcode_fx>()
                .HasMany(e => e.patientdata1)
                .WithRequired(e => e.zipcode_fx1)
                .HasForeignKey(e => e.BillingZipCodePD)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<zipcode_fx>()
                .HasMany(e => e.settlementzipcode_st)
                .WithRequired(e => e.zipcode_fx)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<zipcode_fx>()
                .HasMany(e => e.userdata)
                .WithOptional(e => e.zipcode_fx)
                .HasForeignKey(e => e.ZipCodeUD);
        }
    }
}
