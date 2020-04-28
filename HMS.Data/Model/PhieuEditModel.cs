using System;
using System.Collections.Generic;

namespace HMS.Data.Model
{
    public class PhieuEditModel
    {
        public int Id { get; set; }
        public string KHCode { get; set; }
        public string KHName { get; set; }
        public DateTime Ngay { get; set; }
        public DateTime? NgayDong { get; set; }
        public string SoPhieu { get; set; }
        public string Model { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set; }
        public string BienSo { get; set; }
        public int Km { get; set; }
        public double TongPT { get; set; }
        public double TongCong { get; set; }
        public double Tong { get; set; }
        public int? KTDKyId { get; set; }
        public string KTDKyName { get; set; }
        public int? SuaXeId { get; set; }
        public string SuaXeName { get; set; }
        public int? TiepNhanId { get; set; }
        public string TiepNhanName { get; set; }
        public int? ThuNganId { get; set; }
        public string ThuNganName { get; set; }
        public int? KTraCuoiId { get; set; }
        public string KTraCuoiName { get; set; }
        public string YeuCau { get; set; }
        public string NhanXet { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public DateTime NgaySua { get; set; }

        public int LoaiXeId { get; set; }
        public string LoaiXe { get; set; }
        public int QMSNumber { get; set; }
        public string jsonDichVus { get; set; }
        public string jsonPhuTungs { get; set; }
        public List<PhieuDichVuModel> DichVus { get; set; }
        public List<PhieuPhuTungModel> PhuTungs { get; set; }
        public PhieuEditModel()
        {
            DichVus = new List<PhieuDichVuModel>();
            PhuTungs = new List<PhieuPhuTungModel>();
        }

        public double TongGiaNhap { get; set; }
        public int SLPhuTung { get; set; }
    }

    public class PhieuDichVuModel
    {
        public int? DVId { get; set; }
        public string DVCode { get; set; }
        public string DVName { get; set; }
        public double CKhau { get; set; }
        public double GiaBan { get; set; }
        public double GiaCK { get; set; }
    }

    public class PhieuPhuTungModel
    {
        public int? PTId { get; set; }
        public string PTCode { get; set; }
        public string PTName { get; set; }
        public int SoLuong { get; set; }
        public double CKhau { get; set; }
        public double GiaBan { get; set; }
        public double GiaCK { get; set; }
    }
}
