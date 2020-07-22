using System;

namespace HMS.Data.Model
{
    public class PhieuModel
    {
        public int Id { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string DThoai { get; set; }
        public bool GTinh { get; set; }

        public DateTime? NSinh { get; set; }
        public string DChi { get; set; }
        public string TPho { get; set; }
        public string Huyen { get; set; }
        public string Phuong { get; set; }
        public string Note { get; set; }
        public string NNghiep { get; set; }
        public int? JobId { get; set; }

        public DateTime Ngay { get; set; }
        public string SoPhieu { get; set; }
        public int? ModelId { get; set; }
        public string Model { get; set; }
        public string SoMay { get; set; }
        public string SoKhung { get; set; }
        public string BienSo { get; set; }
        public int Km { get; set; }
        public string Yeucau { get; set; }
        public string Nhanxet { get; set; }
        public string Trangthai { get; set; }
        public string TuVan { get; set; }
        public int TuVanId { get; set; }
        public int WTypeId { get; set; }
        public string LoaiXe { get; set; }

        public double GiaBan { get; set; }
        public double ChietKhau { get; set; }
        public double ThanhTien { get; set; }
        public string GhiChu { get; set; }

        public string TaoKH { get; set; }
        public int QMSServiceId { get; set; }
        public int XeId { get; set; }
        public int Index { get; set; }
        public string strNgay { get; set; }
    }
}
