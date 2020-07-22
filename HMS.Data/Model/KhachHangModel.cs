using System;
using System.Collections.Generic;

namespace HMS.Data.Model
{
    public class _KhachHangModel
    {
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string DThoai { get; set; }
        public bool GTinh { get; set; }
        public DateTime? NSinh { get; set; }
        public string DChi { get; set; }
        public string TPho { get; set; }
        public string Huyen { get; set; }
        public string Phuong { get; set; }
        public string NNghiep { get; set; }
        public int? JobId { get; set; }
        public string Note { get; set; }
    }

    public class KhachHangModel : _KhachHangModel
    {
        public int? XeId { get; set; }
        public DateTime NgayMua { get; set; }
        public string strNgayMua { get; set; }
        public DateTime NgayTao { get; set; }
        public string strNgayTao { get; set; }
        public int Loaixe { get; set; }
        public int Id { get; set; }
        public int? ModelId { get; set; }
        public string strModel { get; set; }
        public string SoMay { get; set; }
        public string SoSuon { get; set; }
        public string BienSo { get; set; }
        public int Km { get; set; }        
        public List<ModelSelectItem> Xes { get; set; }
        public KhachHangModel()
        {
            Xes = new List<ModelSelectItem>();
        }
    }

    public class KhachHangCollectModel
    {
        public List<PhieuMuaModel> All { get; set; }
        public List<PhieuMuaModel> DichVus { get; set; }
        public List<PhieuMuaModel> Buys { get; set; }
        public KhachHangCollectModel()
        {
            All = new List<PhieuMuaModel>();
            DichVus = new List<PhieuMuaModel>();
            Buys = new List<PhieuMuaModel>();
        }
    }
}
