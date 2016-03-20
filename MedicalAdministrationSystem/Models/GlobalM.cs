namespace MedicalAdministrationSystem.Models
{
    public class GlobalM
    {
        public string AccountName { get; set; }
        public int? AccountID { get; set; }
        public int UserID { get; set; }
        public int PriviledgeID { get; set; }
        public bool Secure { get; set; } = false;
        public bool AllSee { get; set; }
    }
}