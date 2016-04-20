namespace MedicalAdministrationSystem.Models
{
    public class GlobalM
    {
        public int? AccountID { get; set; }
        public int? UserID { get; set; }
        public int PriviledgeID { get; set; }
        public bool Secure { get; set; } = false;
        public bool AllSee { get; set; }
        public int? CompanyId { get; set; }
    }
}