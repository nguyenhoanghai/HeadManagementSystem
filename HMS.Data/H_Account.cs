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
    
    public partial class H_Account
    {
        public int Id { get; set; }
        public int NhanVienId { get; set; }
        public string AccName { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public int Role { get; set; }
    
        public virtual H_NhanVien H_NhanVien { get; set; }
    }
}