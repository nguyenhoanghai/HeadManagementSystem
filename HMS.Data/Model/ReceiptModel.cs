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


        //public double GiaBan { get; set; }
        //public double ChietKhau { get; set; }
        //public double ThanhTien { get; set; }
        //public string Note { get; set; }

    }
}
