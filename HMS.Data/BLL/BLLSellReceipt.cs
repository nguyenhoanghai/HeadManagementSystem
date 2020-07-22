using HMS.Data.Model;
using System;
using System.Collections.Generic;
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

        public List<SellReceiptModel> Gets(string connectString, DateTime _from, DateTime _to)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    return (from x in db.H_SellReceipt
                            where !x.IsDeleted && x.CreatedDate >= _from && x.CreatedDate <= _to
                            orderby x.CreatedDate descending
                            select new SellReceiptModel()
                            {
                                Id = x.Id,
                                KHId = x.KHId,
                                Ma = x.H_KhachHang.Code,
                                Ten = x.H_KhachHang.Name,
                                NSinh = x.H_KhachHang.Birthday,
                                GTinh = x.H_KhachHang.Gender,
                                DThoai = x.H_KhachHang.Phone,
                                DChi = x.H_KhachHang.Address,
                                TPho = x.H_KhachHang.City,
                                Huyen = x.H_KhachHang.District,
                                Phuong = x.H_KhachHang.Wards,
                                Model = (x.H_Model != null ? x.H_Model.Note : ""),
                                ModelId = x.ModelId,
                                Note = x.Note,
                                Ngay = x.CreatedDate,
                                SoPhieu = x.SoPhieu,
                                BienSo = x.LicenseNumber,
                                SoMay = x.MachineNumber,
                                SoKhung = x.ChassisNumber,
                                NNghiep = (x.H_KhachHang.H_NgheNghiep != null ? x.H_KhachHang.H_NgheNghiep.Code : ""),
                                JobId = x.H_KhachHang.JobId,
                                GiaBan = x.Price,
                                ChietKhau = x.Discount,
                                ThanhTien = x.Total,
                                WorkTypeId = x.WorkTypeId,
                                MaTuVan = x.H_NhanVien.Code,
                                TenTuVan = x.H_NhanVien.Name,
                                GhiChu = x.Note
                            }).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public SellReceiptModel Get(string connectString, int Id)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    return (from x in db.H_SellReceipt
                            where !x.IsDeleted && x.Id == Id
                            orderby x.CreatedDate descending
                            select new SellReceiptModel()
                            {
                                Id = x.Id,
                                KHId = x.KHId,
                                Ma = x.H_KhachHang.Code,
                                Ten = x.H_KhachHang.Name,
                                NSinh = x.H_KhachHang.Birthday,
                                GTinh = x.H_KhachHang.Gender,
                                DThoai = x.H_KhachHang.Phone,
                                DChi = x.H_KhachHang.Address,
                                TPho = x.H_KhachHang.City,
                                Huyen = x.H_KhachHang.District,
                                Phuong = x.H_KhachHang.Wards,
                                Model = (x.H_Model != null ? x.H_Model.Note : ""),
                                ModelId = x.ModelId,
                                Note = x.Note,
                                Ngay = x.CreatedDate,
                                SoPhieu = x.SoPhieu,
                                BienSo = x.LicenseNumber,
                                SoMay = x.MachineNumber,
                                SoKhung = x.ChassisNumber,
                                NNghiep = (x.H_KhachHang.H_NgheNghiep != null ? x.H_KhachHang.H_NgheNghiep.Code : ""),
                                JobId = x.H_KhachHang.JobId,
                                GiaBan = x.Price,
                                ChietKhau = x.Discount,
                                ThanhTien = x.Total,
                                WorkTypeId = x.WorkTypeId,
                                MaTuVan = x.H_NhanVien.Code,
                                TenTuVan = x.H_NhanVien.Name,
                                GhiChu = x.Note,
                                TuVanId = x.NV_TuVan.Value,
                                SoBaoHanh = x.GuaranteeNumber
                            }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<SellReceiptModel> KhachHangPhieus(string connectString, int khId)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    return (from x in db.H_SellReceipt
                            where !x.IsDeleted && x.KHId == khId
                            orderby x.CreatedDate descending
                            select new SellReceiptModel()
                            {
                                Id = x.Id,
                                KHId = x.KHId,
                                Ma = x.H_KhachHang.Code,
                                Ten = x.H_KhachHang.Name,
                                NSinh = x.H_KhachHang.Birthday,
                                GTinh = x.H_KhachHang.Gender,
                                DThoai = x.H_KhachHang.Phone,
                                DChi = x.H_KhachHang.Address,
                                TPho = x.H_KhachHang.City,
                                Huyen = x.H_KhachHang.District,
                                Phuong = x.H_KhachHang.Wards,
                                Model = (x.H_Model != null ? x.H_Model.Note : ""),
                                ModelId = x.ModelId,
                                Note = x.Note,
                                Ngay = x.CreatedDate,
                                SoPhieu = x.SoPhieu,
                                BienSo = x.LicenseNumber,
                                SoMay = x.MachineNumber,
                                SoKhung = x.ChassisNumber,
                                NNghiep = (x.H_KhachHang.H_NgheNghiep != null ? x.H_KhachHang.H_NgheNghiep.Code : ""),
                                JobId = x.H_KhachHang.JobId,
                                GiaBan = x.Price,
                                ChietKhau = x.Discount,
                                ThanhTien = x.Total,
                                WorkTypeId = x.WorkTypeId,
                                MaTuVan = x.H_NhanVien.Code,
                                TenTuVan = x.H_NhanVien.Name,
                                GhiChu = x.Note
                            }).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

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

        public ResponseModel Insert(string connectString, SellReceiptModel model)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (!CheckExists(model, db))
                {
                    H_SellReceipt sellInfo;
                    double gt = 0;
                    if (model.ChietKhau == 0)
                        gt = model.GiaBan;
                    else
                        gt = (model.GiaBan - ((model.GiaBan * model.ChietKhau) / 100));
                    H_KhachHang khObj = null;
                    if (model.Id == 0)
                    {
                        khObj = new H_KhachHang()
                        {
                            Code = model.Ma,
                            Name = model.Ten,
                            Gender = model.GTinh,
                            Birthday = model.NSinh,
                            Phone = model.DThoai,
                            Address = model.DChi,
                            City = model.TPho,
                            District = model.Huyen,
                            Wards = model.Phuong,
                            JobId = model.JobId,
                            Note = model.GhiChu
                        };
                        khObj.H_SellReceipt = new List<H_SellReceipt>();
                    }

                    sellInfo = new H_SellReceipt()
                    {
                        KHId = model.Id,
                        H_KhachHang = khObj,
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
                        CreatedDate = (model.Ngay.Year == 1 ? DateTime.Now : model.Ngay),
                        Index = model.Index,
                        WorkTypeId = model.WorkTypeId,
                        GuaranteeNumber = model.SoBaoHanh
                    };
                    db.H_SellReceipt.Add(sellInfo);
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

        public ResponseModel Edit(string connectString, SellReceiptModel model)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                var obj = db.H_SellReceipt.FirstOrDefault(x => x.Id == model.Id);
                if (obj != null)
                {
                    double gt = 0;
                    if (model.ChietKhau == 0)
                        gt = model.GiaBan;
                    else
                        gt = (model.GiaBan - ((model.GiaBan * model.ChietKhau) / 100));

                    obj.ModelId = model.ModelId;
                    obj.LicenseNumber = model.BienSo;
                    obj.MachineNumber = model.SoMay;
                    obj.ChassisNumber = model.SoKhung;
                    obj.NV_TuVan = model.TuVanId;
                    obj.Price = model.GiaBan;
                    obj.Discount = model.ChietKhau;
                    obj.Total = gt;
                    obj.Note = model.GhiChu;
                    obj.WorkTypeId = model.WorkTypeId;
                    obj.GuaranteeNumber = model.SoBaoHanh;
                    db.SaveChanges();
                }
                else
                {
                    result.IsSuccess = false;
                    result.sms = "Số phiếu này đã tồn tại trong hệ thống.";
                }
                return result;
            }
        }

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
                                CreatedDate = (model.Ngay.Year == 1 ? DateTime.Now : model.Ngay)
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
        public int GetLastInDay(string connectString, DateTime date)
        {
            using (var db = new HMSEntities(connectString))
            {
                var to = date.AddHours(23).AddMinutes(59);
                var objs = db.H_SellReceipt.Where(x => !x.IsDeleted && x.CreatedDate >= date && x.CreatedDate <= to).OrderByDescending(x => x.Index).Select(x => x.Index).ToList();
                if (objs.Count > 0)
                    return (objs[0] + 1);
                return 1;
            }
        }

    }
}
