﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HMSEntities : DbContext
    {
        public HMSEntities(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<H_Account> H_Account { get; set; }
        public DbSet<H_DichVu> H_DichVu { get; set; }
        public DbSet<H_KhachHang> H_KhachHang { get; set; }
        public DbSet<H_LoaiNhanVien> H_LoaiNhanVien { get; set; }
        public DbSet<H_Model> H_Model { get; set; }
        public DbSet<H_NgheNghiep> H_NgheNghiep { get; set; }
        public DbSet<H_NhanVien> H_NhanVien { get; set; }
        public DbSet<H_PhuTung> H_PhuTung { get; set; }
        public DbSet<H_Receipt_DichVu> H_Receipt_DichVu { get; set; }
        public DbSet<H_Receipt_PT> H_Receipt_PT { get; set; }
        public DbSet<H_Work> H_Work { get; set; }
        public DbSet<H_WorkType> H_WorkType { get; set; }
        public DbSet<H_NhapPT> H_NhapPT { get; set; }
        public DbSet<H_Receiption> H_Receiption { get; set; }
        public DbSet<H_SellReceipt> H_SellReceipt { get; set; }
    }
}
