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
    
    public partial class exceptedschedule
    {
        public int IdES { get; set; }
        public bool IncludedES { get; set; }
        public int UserDataIdES { get; set; }
        public System.DateTime StartDateES { get; set; }
        public System.DateTime FinishDateED { get; set; }
    
        public virtual userdata userdata { get; set; }
    }
}