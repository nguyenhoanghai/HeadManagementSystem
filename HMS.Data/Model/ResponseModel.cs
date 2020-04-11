using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Data.Model
{
   public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string sms { get; set; }
        public string Title { get; set; }
        public dynamic Data { get; set; }
        public dynamic Data_1 { get; set; }
        public dynamic Data_2 { get; set; }
        public dynamic Data_3 { get; set; }
    }
}
