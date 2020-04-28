using System;

namespace HMS.Data.Model
{
    public class ReceiptModel
    {
        public int Id { get; set; }
        public DateTime Ngay { get; set; }
        public string SoPhieu { get; set; }
        public string MaKH { get; set; }
        public string Model { get; set; }
        public int ModelId { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set; }
        public string BienSo { get; set; }
        public int Km { get; set; }
        public string Yeucau { get; set; }
        public string Nhanxet { get; set; }
        public string Trangthai { get; set; }
    }

    public class PhieuMuaModel : ReceiptModel
    { 
        public string KHTen { get; set; }
        public string KHDChi { get; set; }
        public string KHTPho { get; set; }
        public string KHHuyen { get; set; }
        public string KHPhuong { get; set; }
        public string KHDThoai { get; set; }
        public DateTime? KHNSinh { get; set; }
        public string GhiChu { get; set; }
        public int SLMua { get; set; }
        public double TongMua { get; set; }
        public int SLDichVu { get; set; }
        public double TongDichVu { get; set; }
        public DateTime? NgayMuaCuoi { get; set; }
        public bool IsMua { get; set; }
    }

    public class PhieuMoModel : ReceiptModel
    {
        public int KHId { get; set; }
        public string KHTen { get; set; }        
        public string KHDThoai { get; set; }        
        public int SLPhuTung { get; set; }
        public double TongGiaNhap { get; set; }
        public double TongGiaBan { get; set; }
        public double TienCong { get; set; }
        public double TongTien { get; set; }
        public DateTime? NgayDong { get; set; }
    }
}
