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
    
    public partial class H_SellReceipt
    {
        public H_SellReceipt()
        {
            this.H_Receiption = new HashSet<H_Receiption>();
        }
    
        public int Id { get; set; }
        public int KHId { get; set; }
        public int ModelId { get; set; }
        public string SoPhieu { get; set; }
        public Nullable<int> NV_TuVan { get; set; }
        public string MachineNumber { get; set; }
        public string ChassisNumber { get; set; }
        public string LicenseNumber { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public string Note { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int WorkTypeId { get; set; }
        public int Index { get; set; }
        public string GuaranteeNumber { get; set; }
    
        public virtual H_KhachHang H_KhachHang { get; set; }
        public virtual H_Model H_Model { get; set; }
        public virtual H_NhanVien H_NhanVien { get; set; }
        public virtual ICollection<H_Receiption> H_Receiption { get; set; }
        public virtual H_WorkType H_WorkType { get; set; }
    }
}
