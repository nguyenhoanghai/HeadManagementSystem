using HMS.Data.Model;
using System;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLSellReceipt
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLSellReceipt _Instance;  //volatile =>  tranh dung thread
        public static BLLSellReceipt Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLSellReceipt();

                return _Instance;
            }
        }
        private BLLSellReceipt() { }
        #endregion

        //public List<SellReceiptModel> Gets(string connectString)
        //{
        //    using (var db = new HMSEntities(connectString))
        //    {
        //        try
        //        {
        //            return (from x in db.H_SellReceipt
        //                    where !x.IsDeleted 
        //                    select new SellReceiptModel()
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

        //public  SellReceiptModel  GetByCode(string connectString,string code)
        //{
        //    using (var db = new HMSEntities(connectString))
        //    {
        //        try
        //        {
        //            return (from x in db.H_SellReceipt
        //                    where !x.IsDeleted && x.Code.Trim().ToUpper() == code.Trim().ToUpper()
        //                    select new SellReceiptModel()
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
        //          //  return db.H_SellReceipt.Where(x => !x.IsDeleted && !x.H_LoaiNhanVien.IsDeleted && x.LoaiNVId == loaiNV).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code }).ToList();
        //        return db.H_SellReceipt.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code }).ToList();
        //    }
        //}

        public ResponseModel InsertOrUpdate(string connectString, SellReceiptModel model)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (!CheckExists(model, db))
                {
                    H_SellReceipt sellInfo;
                    if (model.Id == 0)
                    {
                        var _khIds = (from x in db.H_KhachHang where !x.IsDeleted && x.Code.Trim().ToUpper() == model.MaKH.Trim().ToUpper() select x.Id).ToList();
                        if (_khIds.Count == 0)
                        {
                            result.IsSuccess = false;
                            result.sms = "Khách hàng đã bị xóa hoặc không tồn tại trong hệ thống.!";
                        }
                        else
                        {
                            double gt = 0;
                            if (model.ChietKhau == 0)
                                gt = model.GiaBan;
                            else
                                gt = (model.GiaBan - ((model.GiaBan * model.ChietKhau) / 100));
                            sellInfo = new H_SellReceipt()
                            {
                                KHId = _khIds[0],
                                SoPhieu = model.SoPhieu,
                                ModelId = model.ModelId,
                                LicenseNumber = model.BienSo,
                                MachineNumber = model.SoMay,
                                ChassisNumber = model.SoKhung,
                                NV_TuVan = model.TuVanId,
                                Price = model.GiaBan,
                                Discount = model.ChietKhau,
                                Total = gt,
                                Note = model.Note,
                                CreatedDate = (model.Ngay.Year == 1?DateTime.Now:model.Ngay) 
                            };
                            db.H_SellReceipt.Add(sellInfo);
                        }
                    }
                    else
                    {
                        sellInfo = db.H_SellReceipt.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                        if (sellInfo != null)
                        {
                            sellInfo.SoPhieu = model.SoPhieu;
                            sellInfo.Price = model.GiaBan;
                            sellInfo.Discount = model.ChietKhau;
                            sellInfo.Total = model.ThanhTien; 
                            sellInfo.ModelId = model.ModelId;
                            sellInfo.Note = model.Note; 
                            sellInfo.NV_TuVan = model.TuVanId;
                            sellInfo.LicenseNumber = model.BienSo;
                            sellInfo.MachineNumber = model.SoMay;
                            sellInfo.ChassisNumber = model.SoKhung;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.sms = "Thông tin phiếu đã bị xóa hoặc không tồn tại trong hệ thống.!";
                        }
                    }
                    if (result.IsSuccess)
                        db.SaveChanges();
                }
                else
                {
                    result.IsSuccess = false;
                    result.sms = "Số phiếu này đã tồn tại trong hệ thống. Vui lòng nhập lại số phiếu khác.";
                }
                return result;
            }
        }

        public bool Delete(string connectString, int Id)
        {
            using (var db = new HMSEntities(connectString))
            {
                var obj = db.H_SellReceipt.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CheckExists(SellReceiptModel SellReceipt, HMSEntities db)
        {
            H_SellReceipt obj = null;
            if (!string.IsNullOrEmpty(SellReceipt.SoPhieu))
                obj = db.H_SellReceipt.FirstOrDefault(x => !x.IsDeleted && x.Id != SellReceipt.Id && x.SoPhieu.Trim().ToUpper().Equals(SellReceipt.SoPhieu.Trim().ToUpper()));
            return obj != null ? true : false;
        }
         
    }
}
