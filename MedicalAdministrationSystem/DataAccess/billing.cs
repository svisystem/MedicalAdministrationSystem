//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedicalAdministrationSystem.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class billing
    {
        public int IdB { get; set; }
        public int PatientIdB { get; set; }
        public int UserIdB { get; set; }
        public int CompanyIdFromB { get; set; }
        public Nullable<int> CompanyIdToB { get; set; }
        public string CodeB { get; set; }
        public System.DateTime DateTimeB { get; set; }
        public byte[] BillB { get; set; }
    
        public virtual companydata companydata { get; set; }
        public virtual companydata companydata1 { get; set; }
        public virtual patientdata patientdata { get; set; }
        public virtual userdata userdata { get; set; }
    }
}
