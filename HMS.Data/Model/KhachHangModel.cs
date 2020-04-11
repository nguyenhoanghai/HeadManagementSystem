using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Data.Model
{
   public class KhachHangModel
    {
        public int Id { get; set; }
        public int? ModelId { get; set; }
        public string strModel { get; set; }
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
        public string SoMay { get; set; }
        public string SoSuon { get; set; }
        public string BienSo { get; set; }
        public int Km { get; set; }
        public string NNghiep { get; set; }
        public int? JobId { get; set; }
    }
}
