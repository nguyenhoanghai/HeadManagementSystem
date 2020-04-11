using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Data.Model
{
   public class SellReceiptModel
    {
        public int Id { get; set; }
        public string SoPhieu { get; set; }
        public string MaKH { get; set; }
        public string Model { get; set; }
        public int ModelId { get; set; }
        public string TuVan { get; set; }
        public int TuVanId { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set; }
        public string BienSo { get; set; }
        public double GiaBan { get; set; }
        public double ChietKhau { get; set; }
        public double ThanhTien { get; set; } 
        public string Note { get; set; }
        public DateTime Ngay { get; set; } 
    }
}
