using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Data.Model
{
   public class NhanVienModel
    {
        public int Id { get; set; }
        public int LoaiNV { get; set; }
        public string strLoaiNV { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public string Note { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
    }
}
