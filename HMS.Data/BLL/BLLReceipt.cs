using GPRO.Ultilities;
using HMS.Data.Model;
using System;
using System.Collections.Generic;
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

        public List<PhieuMuaModel> Gets(string connectString, DateTime _from, DateTime _to)
        {
            List<PhieuMuaModel> phieus = new List<PhieuMuaModel>();
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    var dv = (from x in db.H_Receiption
                              where !x.IsDeleted && x.Date >= _from && x.Date <= _to
                              orderby x.CreatedDate descending
                              select new PhieuMuaModel()
                              {
                                  Id = x.Id,
                                  MaKH = x.H_KhachHang.Code,
                                  KHTen = x.H_KhachHang.Name,
                                  Ngay = x.Date,
                                  KHNSinh = x.H_KhachHang.Birthday,
                                  KHDThoai = x.H_KhachHang.Phone,
                                  KHDChi = x.H_KhachHang.Address,
                                  KHTPho = x.H_KhachHang.City,
                                  KHHuyen = x.H_KhachHang.District,
                                  KHPhuong = x.H_KhachHang.Wards,
                                  GhiChu = x.Note,
                                  SLDichVu = 1,
                                  TongDichVu = x.Total,
                                  IsMua = false
                              }).ToList();

                    dv.AddRange((from x in db.H_SellReceipt
                                 where !x.IsDeleted && x.CreatedDate >= _from && x.CreatedDate <= _to
                                 select new PhieuMuaModel()
                                 {
                                     Id = x.Id,
                                     MaKH = x.H_KhachHang.Code,
                                     KHTen = x.H_KhachHang.Name,
                                     Ngay = x.CreatedDate,
                                     KHNSinh = x.H_KhachHang.Birthday,
                                     KHDThoai = x.H_KhachHang.Phone,
                                     KHDChi = x.H_KhachHang.Address,
                                     KHTPho = x.H_KhachHang.City,
                                     KHHuyen = x.H_KhachHang.District,
                                     KHPhuong = x.H_KhachHang.Wards,
                                     GhiChu = x.Note,
                                     SLMua = 1,
                                     TongMua = x.Total,
                                     NgayMuaCuoi = x.CreatedDate,
                                     IsMua = true
                                 }).ToList());
                    var maKHs = dv.Select(x => x.MaKH).Distinct().ToList();
                    for (int i = 0; i < maKHs.Count; i++)
                    {
                        var founds = dv.Where(x => x.MaKH == maKHs[i]).ToList();
                        var kh = new PhieuMuaModel();
                        Parse.CopyObject(founds[0], ref kh);
                        kh.SLDichVu = founds.Count(x => !x.IsMua);
                        kh.TongDichVu = founds.Where(x => !x.IsMua).Sum(x => x.TongDichVu);
                        var khmua = founds.Where(x => x.IsMua).OrderByDescending(x => x.Ngay).ToList();
                        kh.SLMua = khmua.Count;
                        kh.TongMua = khmua.Sum(x => x.TongDichVu);
                        if (khmua.Count > 0)
                            kh.NgayMuaCuoi = khmua[0].Ngay;
                        phieus.Add(kh);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return phieus;
        }

        public List<PhieuMoModel> PhieuMos(string connectString)
        {
            var phieus = new List<PhieuMoModel>();
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    phieus.AddRange((from x in db.H_Receiption
                                     where !x.IsDeleted && !x.ClosedDate.HasValue
                                     orderby x.CreatedDate descending
                                     select new PhieuMoModel()
                                     {
                                         Id = x.Id,
                                         SoPhieu = x.Code,
                                         KHId = x.KHId,
                                         MaKH = x.H_KhachHang.Code,
                                         KHTen = x.H_KhachHang.Name,
                                         KHDThoai = x.H_KhachHang.Phone,
                                         Model = x.H_Model.Note,
                                         SoMay = x.MachineNumber,
                                         SoKhung = x.ChassisNumber,
                                         BienSo = x.LicenseNumber,
                                         TongGiaNhap = x.Total_NhapPT,
                                         SLPhuTung = x.SL_PT,
                                         TongGiaBan = x.Total_PTung,
                                         TienCong = x.Total_Cong,
                                         TongTien = x.Total,
                                         Yeucau = x.Required,
                                         Ngay = x.CreatedDate
                                     }).ToList());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return phieus;
        }

        public List<PhieuMoModel> PhieuDongs(string connectString)
        {
            var phieus = new List<PhieuMoModel>();
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    phieus.AddRange((from x in db.H_Receiption
                                     where !x.IsDeleted && x.ClosedDate.HasValue
                                     orderby x.CreatedDate descending
                                     select new PhieuMoModel()
                                     {
                                         Id = x.Id,
                                         SoPhieu = x.Code,
                                         KHId = x.KHId,
                                         MaKH = x.H_KhachHang.Code,
                                         KHTen = x.H_KhachHang.Name,
                                         KHDThoai = x.H_KhachHang.Phone,
                                         Model = x.H_Model.Note,
                                         SoMay = x.MachineNumber,
                                         SoKhung = x.ChassisNumber,
                                         BienSo = x.LicenseNumber,
                                         TongGiaNhap = x.Total_NhapPT,
                                         SLPhuTung = x.SL_PT,
                                         TongGiaBan = x.Total_PTung,
                                         TienCong = x.Total_Cong,
                                         TongTien = x.Total,
                                         Yeucau = x.Required,
                                         Ngay = x.CreatedDate,
                                         NgayDong = x.ClosedDate.Value
                                     }).ToList());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return phieus;
        }

        public List<PhieuEditModel> KhachHangPhieus(string connectString,int khId)
        {
            var phieus = new List<PhieuEditModel>();
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    phieus.AddRange((from x in db.H_Receiption
                                     where !x.IsDeleted && x.KHId == khId
                                     orderby x.CreatedDate descending
                                     select new PhieuEditModel()
                                     {
                                         Id = x.Id, 
                                         Ngay = x.CreatedDate,
                                         NgayDong = x.ClosedDate,
                                         SoPhieu = x.Code,
                                         Model = x.H_Model.Note,
                                         SoKhung = x.ChassisNumber,
                                         SoMay = x.MachineNumber,
                                         Km = x.New_Km,
                                         BienSo = x.LicenseNumber,
                                         KTDKyId = x.NV_KTTruong,
                                         KTDKyName = (x.NV_TiepNhan.HasValue ? x.H_NhanVien3.Name : ""),
                                         SuaXeId = x.NV_SuaXe,
                                         SuaXeName = (x.NV_TiepNhan.HasValue ? x.H_NhanVien1.Name : ""),
                                         TiepNhanId = x.NV_TiepNhan,
                                         TiepNhanName = (x.NV_TiepNhan.HasValue ? x.H_NhanVien.Name : ""),
                                         ThuNganId = x.NV_ThuNgan,
                                         ThuNganName = (x.NV_TiepNhan.HasValue ? x.H_NhanVien2.Name : ""),
                                         KTraCuoiId = x.NV_KTCuoi,
                                         KTraCuoiName = (x.NV_TiepNhan.HasValue ? x.H_NhanVien4.Name : ""),
                                         YeuCau = x.Required,
                                         NhanXet = x.CheckResult,
                                         TrangThai = x.StatusBefore,
                                         GhiChu = x.Note,
                                         NgaySua = x.Date,
                                         LoaiXeId = x.WorkTypeId,
                                         LoaiXe = x.H_WorkType.Name,
                                         QMSNumber = x.QMSNumber,
                                         Tong = x.Total,
                                         TongCong = x.Total_Cong,
                                         TongPT = x.Total_PTung,     
                                         TongGiaNhap = x.Total_NhapPT,
                                         SLPhuTung = x.SL_PT
                                     }).ToList());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return phieus;
        }
        
        public PhieuEditModel Get(string connectString, int id)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    var phieu = (from x in db.H_Receiption
                                 where !x.IsDeleted && x.Id == id
                                 select new PhieuEditModel()
                                 {
                                     Id = x.Id,
                                     KHCode = x.H_KhachHang.Code,
                                     KHName = x.H_KhachHang.Name,
                                     Ngay = x.CreatedDate,
                                     NgayDong = x.ClosedDate,
                                     SoPhieu = x.Code,
                                     Model = x.H_Model.Note,
                                     SoKhung = x.ChassisNumber,
                                     SoMay = x.MachineNumber,
                                     Km = x.New_Km,
                                     BienSo = x.LicenseNumber,
                                     KTDKyId = x.NV_KTTruong,
                                     KTDKyName = (x.NV_TiepNhan.HasValue ? x.H_NhanVien3.Name : ""),
                                     SuaXeId = x.NV_SuaXe,
                                     SuaXeName = (x.NV_TiepNhan.HasValue ? x.H_NhanVien1.Name : ""),
                                     TiepNhanId = x.NV_TiepNhan,
                                     TiepNhanName = (x.NV_TiepNhan.HasValue ? x.H_NhanVien.Name : ""),
                                     ThuNganId = x.NV_ThuNgan,
                                     ThuNganName = (x.NV_TiepNhan.HasValue ? x.H_NhanVien2.Name : ""),
                                     KTraCuoiId = x.NV_KTCuoi,
                                     KTraCuoiName = (x.NV_TiepNhan.HasValue ? x.H_NhanVien4.Name : ""),
                                     YeuCau = x.Required,
                                     NhanXet = x.CheckResult,
                                     TrangThai = x.StatusBefore,
                                     GhiChu = x.Note,
                                     NgaySua = x.Date,
                                     LoaiXeId = x.WorkTypeId,
                                     LoaiXe = x.H_WorkType.Name,
                                     QMSNumber = x.QMSNumber,
                                     Tong = x.Total,
                                     TongCong = x.Total_Cong,
                                     TongPT = x.Total_PTung
                                 }).FirstOrDefault();
                    phieu.DichVus = (from x in db.H_Receipt_DichVu
                                     where !x.IsDeleted && !x.H_DichVu.IsDeleted && x.ReceiptId == id
                                     select new PhieuDichVuModel()
                                     {
                                         DVId = x.DichVuId,
                                         CKhau = x.Percent,
                                         DVCode = x.H_DichVu.H_Work.Code,
                                         DVName = x.H_DichVu.H_Work.Name,
                                         GiaBan = x.Price,
                                         GiaCK = x.PricePercent
                                     }).ToList();
                    phieu.PhuTungs = (from x in db.H_Receipt_PT
                                      where !x.IsDeleted && !x.H_PhuTung.IsDeleted && x.ReceiptId == id
                                      select new PhieuPhuTungModel()
                                      {
                                          PTId = x.PT_Id,
                                          CKhau = x.Percent,
                                          PTCode = x.H_PhuTung.Code,
                                          PTName = x.H_PhuTung.Name,
                                          SoLuong = x.Quantities,
                                          GiaBan = x.Price,
                                          GiaCK = x.PricePercent
                                      }).ToList();
                    return phieu;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public ResponseModel Insert(string connectString, PhieuModel model)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (!CheckExists(model, db))
                {
                    try
                    {
                        H_Receiption receiptInfo;
                        H_KhachHang khObj = null;
                        receiptInfo = new H_Receiption();
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
                                Note = model.Note
                            };
                            khObj.H_Receiption = new List<H_Receiption>();
                        }

                        receiptInfo.KHId = model.Id;
                        receiptInfo.H_KhachHang = khObj;
                        receiptInfo.Code = model.SoPhieu;
                        receiptInfo.ModelId = model.ModelId;
                        receiptInfo.LicenseNumber = model.BienSo;
                        receiptInfo.MachineNumber = model.SoMay;
                        receiptInfo.ChassisNumber = model.SoKhung;
                        receiptInfo.New_Km = model.Km;
                        receiptInfo.Required = model.Yeucau;
                        receiptInfo.CheckResult = model.Nhanxet;
                        receiptInfo.StatusBefore = model.Trangthai;
                        receiptInfo.Note = model.GhiChu;
                        receiptInfo.WorkTypeId = model.WTypeId;
                        receiptInfo.CreatedDate = DateTime.Now;
                        receiptInfo.Date = (model.Ngay.Year == 1 ? DateTime.Now : model.Ngay);
                        receiptInfo.QMSServiceId = model.QMSServiceId;
                        receiptInfo.Index = model.Index;
                        if (model.XeId > 0)
                        {
                            receiptInfo.SellReceiptId = model.XeId;
                            var xe = db.H_SellReceipt.FirstOrDefault(x => !x.IsDeleted && x.Id == model.XeId);
                            if (xe != null)
                                xe.LicenseNumber = model.BienSo;
                        }

                        db.H_Receiption.Add(receiptInfo);
                        if (result.IsSuccess)
                            db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.sms = "Số phiếu này đã tồn tại trong hệ thống. Vui lòng nhập lại số phiếu khác.";
                }
                return result;
            }
        }

        public ResponseModel Edit(string connectString, PhieuEditModel model)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    H_Receiption receiptInfo = db.H_Receiption.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                    if (receiptInfo != null)
                    {
                        receiptInfo.NV_KTTruong = model.KTDKyId;
                        receiptInfo.NV_SuaXe = model.SuaXeId;
                        receiptInfo.NV_ThuNgan = model.ThuNganId;
                        receiptInfo.NV_TiepNhan = model.TiepNhanId;
                        receiptInfo.NV_KTCuoi = model.KTraCuoiId;
                        receiptInfo.Required = model.YeuCau;
                        receiptInfo.CheckResult = model.NhanXet;
                        receiptInfo.StatusBefore = model.TrangThai;
                        receiptInfo.Note = model.GhiChu;
                        receiptInfo.Total_Cong = model.DichVus.Sum(x => x.GiaCK);
                        receiptInfo.Total_PTung = model.PhuTungs.Sum(x => x.GiaCK);
                        receiptInfo.Total = receiptInfo.Total_Cong + receiptInfo.Total_PTung;
                        receiptInfo.Total_NhapPT = model.PhuTungs.Sum(x => x.GiaBan * x.SoLuong);
                        receiptInfo.SL_PT = model.PhuTungs.Count(x => x.PTId != null);
                        receiptInfo.Date = (model.NgaySua.Year == 1 ? DateTime.Now : model.NgaySua);
                        string query = "";
                        if (model.DichVus != null && model.DichVus.Count > 0)
                        {
                            query += (" update [H_Receipt_DichVu] set isdeleted=1 where ReceiptId=" + model.Id);

                            receiptInfo.H_Receipt_DichVu = new List<H_Receipt_DichVu>();
                            for (int i = 0; i < model.DichVus.Count; i++)
                            {
                                if (model.DichVus[i].DVId > 0)
                                    query += " INSERT INTO [dbo].[H_Receipt_DichVu] ([ReceiptId],[DichVuId],[Index],[Price],[Percent],[PricePercent],[IsDeleted]) VALUES (" + model.Id + "," + model.DichVus[i].DVId + ",1,'" + model.DichVus[i].GiaBan.ToString().Replace(',', '.') + "','" + model.DichVus[i].CKhau.ToString().Replace(',', '.') + "','" + model.DichVus[i].GiaCK.ToString().Replace(',', '.') + "',0)";
                            }

                        }
                        if (model.PhuTungs != null && model.PhuTungs.Count > 0)
                        {
                            query += (" update [H_Receipt_PT] set isdeleted=1 where ReceiptId=" + model.Id);

                            receiptInfo.H_Receipt_DichVu = new List<H_Receipt_DichVu>();
                            for (int i = 0; i < model.PhuTungs.Count; i++)
                            {
                                if (model.PhuTungs[i].PTId > 0)
                                    query += " INSERT INTO [dbo].[H_Receipt_PT] ([ReceiptId],[PT_Id],[Index],[Quantities],[Price],[Percent],[PricePercent],[Total],[IsDeleted]) VALUES (" + model.Id + "," + model.PhuTungs[i].PTId + ",1," + model.PhuTungs[i].SoLuong + ",'" + model.PhuTungs[i].GiaBan.ToString().Replace(',','.') + "','" + model.PhuTungs[i].CKhau.ToString().Replace(',', '.') + "','" + model.PhuTungs[i].GiaCK.ToString().Replace(',', '.') + "','" + (model.PhuTungs[i].SoLuong * model.PhuTungs[i].GiaCK).ToString().Replace(',', '.') + "',0)";
                            }
                        }
                        if (!string.IsNullOrEmpty(query))
                            db.Database.ExecuteSqlCommand(query);
                        db.SaveChanges();
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.sms = "Phiếu dịch vụ này đã bị xóa hoặc không tồn tại trong hệ thống. Vui lòng kiểm tra lại.";
                    }
                }
                catch (Exception ex)
                {
                }
                return result;
            }
        }

        public bool Delete(string connectString, int Id)
        {
            using (var db = new HMSEntities(connectString))
            {
                var obj = db.H_Receiption.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CheckExists(PhieuModel Receipt, HMSEntities db)
        {
            H_Receiption obj = null;
            if (!string.IsNullOrEmpty(Receipt.SoPhieu))
                obj = db.H_Receiption.FirstOrDefault(x => !x.IsDeleted && x.Id != Receipt.Id && x.Code.Trim().ToUpper().Equals(Receipt.SoPhieu.Trim().ToUpper()));
            return obj != null ? true : false;
        }

        public int GetLastInDay(string connectString, DateTime date)
        {
            using (var db = new HMSEntities(connectString))
            {
                var to = date.AddHours(23);
                var objs = db.H_Receiption.Where(x => !x.IsDeleted && x.CreatedDate >= date && x.CreatedDate <= to).OrderByDescending(x => x.CreatedDate).Select(x => x.Index).ToList();
                if (objs.Count > 0)
                    return (objs[0] + 1);
                return 1;
            }
        }

        public bool Close(string connectString, int Id)
        {
            using (var db = new HMSEntities(connectString))
            {
                var obj = db.H_Receiption.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.ClosedDate = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }


    }
}
