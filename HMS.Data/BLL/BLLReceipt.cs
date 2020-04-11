using HMS.Data.Model;
using System;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLReceipt
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLReceipt _Instance;  //volatile =>  tranh dung thread
        public static BLLReceipt Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLReceipt();

                return _Instance;
            }
        }
        private BLLReceipt() { }
        #endregion

        //public List<ReceiptModel> Gets(string connectString)
        //{
        //    using (var db = new HMSEntities(connectString))
        //    {
        //        try
        //        {
        //            return (from x in db.H_Receipt
        //                    where !x.IsDeleted 
        //                    select new ReceiptModel()
        //                    {
        //                        Id = x.Id,
        //                        Ma = x.Code,
        //                        Ten = x.Name,
        //                        NSinh = x.Birthday,
        //                        GTinh = x.Gender,
        //                        DThoai = x.Phone,
        //                        DChi = x.Address,
        //                        TPho = x.City,
        //                        Huyen = x.District,
        //                        Phuong = x.Wards,
        //                        strModel = (x.H_Model != null ? x.H_Model.Note : ""),
        //                        ModelId = x.ModelId,
        //                        Note = x.Note,
        //                        BienSo = x.LicenseNumber,
        //                        SoMay = x.MachineNumber,
        //                        SoSuon = x.ChassisNumber,
        //                        Km = x.Km,
        //                        NNghiep = (x.H_NgheNghiep != null ? x.H_NgheNghiep.Code : ""),
        //                        JobId = x.JobId
        //                    }).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        //public  ReceiptModel  GetByCode(string connectString,string code)
        //{
        //    using (var db = new HMSEntities(connectString))
        //    {
        //        try
        //        {
        //            return (from x in db.H_Receipt
        //                    where !x.IsDeleted && x.Code.Trim().ToUpper() == code.Trim().ToUpper()
        //                    select new ReceiptModel()
        //                    {
        //                        Id = x.Id,
        //                        Ma = x.Code,
        //                        Ten = x.Name,
        //                        NSinh = x.Birthday,
        //                        GTinh = x.Gender,
        //                        DThoai = x.Phone,
        //                        DChi = x.Address,
        //                        TPho = x.City,
        //                        Huyen = x.District,
        //                        Phuong = x.Wards,
        //                        strModel = (x.H_Model != null ? x.H_Model.Note : ""),
        //                        ModelId = x.ModelId,
        //                        Note = x.Note,
        //                        BienSo = x.LicenseNumber,
        //                        SoMay = x.MachineNumber,
        //                        SoSuon = x.ChassisNumber,
        //                        Km = x.Km,
        //                        NNghiep = (x.H_NgheNghiep != null ? x.H_NgheNghiep.Code : ""),
        //                        JobId = x.JobId
        //                    }).FirstOrDefault();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        //public List<ModelSelectItem> GetLookUp(string connectString, int loaiNV)
        //{
        //    using (var db = new HMSEntities(connectString))
        //    {
        //        //if (loaiNV != 0)
        //          //  return db.H_Receipt.Where(x => !x.IsDeleted && !x.H_LoaiNhanVien.IsDeleted && x.LoaiNVId == loaiNV).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code }).ToList();
        //        return db.H_Receipt.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code }).ToList();
        //    }
        //}

        //public ResponseModel InsertOrUpdate(string connectString, ReceiptModel model)
        //{
        //    var result = new ResponseModel();
        //    result.IsSuccess = true;
        //    using (var db = new HMSEntities(connectString))
        //    {
        //        if (!CheckExists(model, db))
        //        {
        //            H_Receipt receiptInfo;
        //            if (model.Id == 0)
        //            {
        //                var _khIds = (from x in db.H_KhachHang where !x.IsDeleted && x.Code.Trim().ToUpper() == model.MaKH.Trim().ToUpper() select x.Id).ToList();
        //                if (_khIds.Count == 0)
        //                {
        //                    result.IsSuccess = false;
        //                    result.sms = "Khách hàng đã bị xóa hoặc không tồn tại trong hệ thống.!";
        //                }
        //                else
        //                { 
        //                    receiptInfo = new H_Receipt()
        //                    {
        //                        KHId = _khIds[0],
        //                        Code = model.SoPhieu,
        //                        ModelId = model.ModelId,
        //                        LicenseNumber = model.BienSo,
        //                        MachineNumber = model.SoMay,
        //                        ChassisNumber = model.SoKhung,
        //                        NV_TuVan = model.TuVanId,
        //                        Price = model.GiaBan,
        //                        Discount = model.ChietKhau,
        //                        Total = gt,
        //                        Note = model.Note,
        //                        CreatedDate = (model.Ngay.Year == 1?DateTime.Now:model.Ngay) 
        //                    };
        //                    db.H_Receipt.Add(receiptInfo);
        //                }
        //            }
        //            else
        //            {
        //                receiptInfo = db.H_Receipt.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
        //                if (receiptInfo != null)
        //                {
        //                    receiptInfo.SoPhieu = model.SoPhieu;
        //                    receiptInfo.Price = model.GiaBan;
        //                    receiptInfo.Discount = model.ChietKhau;
        //                    receiptInfo.Total = model.ThanhTien; 
        //                    receiptInfo.ModelId = model.ModelId;
        //                    receiptInfo.Note = model.Note; 
        //                    receiptInfo.NV_TuVan = model.TuVanId;
        //                    receiptInfo.LicenseNumber = model.BienSo;
        //                    receiptInfo.MachineNumber = model.SoMay;
        //                    receiptInfo.ChassisNumber = model.SoKhung;
        //                }
        //                else
        //                {
        //                    result.IsSuccess = false;
        //                    result.sms = "Thông tin phiếu đã bị xóa hoặc không tồn tại trong hệ thống.!";
        //                }
        //            }
        //            if (result.IsSuccess)
        //                db.SaveChanges();
        //        }
        //        else
        //        {
        //            result.IsSuccess = false;
        //            result.sms = "Số phiếu này đã tồn tại trong hệ thống. Vui lòng nhập lại số phiếu khác.";
        //        }
        //        return result;
        //    }
        //}

        public bool Delete(string connectString, int Id)
        {
            using (var db = new HMSEntities(connectString))
            {
                var obj = db.H_Receipt.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CheckExists(ReceiptModel Receipt, HMSEntities db)
        {
            H_Receipt obj = null;
            if (!string.IsNullOrEmpty(Receipt.SoPhieu))
                obj = db.H_Receipt.FirstOrDefault(x => !x.IsDeleted && x.Id != Receipt.Id && x.Code.Trim().ToUpper().Equals(Receipt.SoPhieu.Trim().ToUpper()));
            return obj != null ? true : false;
        }
         
    }
}
