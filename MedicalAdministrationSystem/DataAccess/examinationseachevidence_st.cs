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
    
    public partial class examinationseachevidence_st
    {
        public int IdED { get; set; }
        public int IdEX { get; set; }
        public Nullable<bool> Dummy { get; set; }
    
        public virtual evidencedata evidencedata { get; set; }
        public virtual examinationdata examinationdata { get; set; }
    }
}