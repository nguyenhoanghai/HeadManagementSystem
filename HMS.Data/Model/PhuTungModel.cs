using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Data.Model
{
   public class PhuTungModel
    {
        public int Id { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public int SoLuong { get; set; }
        public double GiaMua { get; set; }
        public double GiaBan { get; set; }
        public string Note { get; set; }
    }
}
