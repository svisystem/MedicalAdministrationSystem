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
    
    public partial class belong_st
    {
        public int IdPD { get; set; }
        public int IdUD { get; set; }
        public System.DateTime WhenBelongBS { get; set; }
        public Nullable<bool> Dummy { get; set; }
    
        public virtual userdata userdata { get; set; }
        public virtual patientdata patientdata { get; set; }
    }
}
