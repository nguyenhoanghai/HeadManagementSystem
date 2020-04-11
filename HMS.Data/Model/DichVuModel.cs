using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Data.Model
{
   public class DichVuModel
    {
        public int Id { get; set; }
        public string LoaiCV { get; set; }
        public string MaCV { get; set; }
        public string TenCV { get; set; } 
        public double GiaMua { get; set; }
        public double GiaBan { get; set; }
        public string Note { get; set; }
        public int LoaiCVId { get; set; }
        public int CVId { get; set; }
        public DateTime TGXL { get; set; }
        public string strTGXL { get; set; }
    }
}
