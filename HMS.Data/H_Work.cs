//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HMS.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class H_Work
    {
        public H_Work()
        {
            this.H_DichVu = new HashSet<H_DichVu>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual ICollection<H_DichVu> H_DichVu { get; set; }
    }
}