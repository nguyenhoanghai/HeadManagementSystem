using GPRO.Ultilities;
using HMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLKhachHang
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLKhachHang _Instance;  //volatile =>  tranh dung thread
        public static BLLKhachHang Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLKhachHang();

                return _Instance;
            }
        }
        private BLLKhachHang() { }
        #endregion

        public List<KhachHangModel> Gets(string connectString)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    return (from x in db.H_KhachHang
                            where !x.IsDeleted
                            select new KhachHangModel()
                            {
                                Id = x.Id,
                                Ma = x.Code,
                                Ten = x.Name,
                                NSinh = x.Birthday,
                                GTinh = x.Gender,
                                DThoai = x.Phone,
                                DChi = x.Address,
                                TPho = x.City,
                                Huyen = x.District,
                                Phuong = x.Wards,
                                Note = x.Note,
                                NNghiep = (x.H_NgheNghiep != null ? x.H_NgheNghiep.Code : ""),
                                JobId = x.JobId
                            }).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public  KhachHangModel  Get(string connectString,int Id)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    return (from x in db.H_KhachHang
                            where !x.IsDeleted && x.Id == Id
                            select new KhachHangModel()
                            {
                                Id = x.Id,
                                Ma = x.Code,
                                Ten = x.Name,
                                NSinh = x.Birthday,
                                GTinh = x.Gender,
                                DThoai = x.Phone,
                                DChi = x.Address,
                                TPho = x.City,
                                Huyen = x.District,
                                Phuong = x.Wards,
                                Note = x.Note,
                                NNghiep = (x.H_NgheNghiep != null ? x.H_NgheNghiep.Code : ""),
                                JobId = x.JobId
                            }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public KhachHangModel GetByCode(string connectString, string code)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    var khs = (from x in db.H_KhachHang
                               where !x.IsDeleted && x.Code.Trim().ToUpper() == code.Trim().ToUpper()
                               select new KhachHangModel()
                               {
                                   Id = x.Id,
                                   Ma = x.Code,
                                   Ten = x.Name,
                                   NSinh = x.Birthday,
                                   GTinh = x.Gender,
                                   DThoai = x.Phone,
                                   DChi = x.Address,
                                   TPho = x.City,
                                   Huyen = x.District,
                                   Phuong = x.Wards,
                                   Note = x.Note,
                                   NNghiep = (x.H_NgheNghiep != null ? x.H_NgheNghiep.Code : ""),
                                   JobId = x.JobId
                               }).ToList();
                    if (khs.Count > 0)
                    {
                        var kh = khs.FirstOrDefault();
                        var xes = (from x in db.H_SellReceipt
                                   where !x.IsDeleted && !x.H_KhachHang.IsDeleted && x.KHId == kh.Id
                                   orderby x.CreatedDate descending
                                   select new
                                   {
                                       Id = x.Id,
                                       Name = x.H_Model.Note,
                                       Data1 = x.MachineNumber,
                                       Data2 = x.ChassisNumber,
                                       Data3 = x.LicenseNumber,
                                       Data = x.ModelId,
                                       Record = x.WorkTypeId,
                                       Data4 = x.CreatedDate
                                   }).ToList();
                        if (xes.Count > 0)
                        {
                            for (int i = 0; i < xes.Count; i++)
                            {
                                var obj = new ModelSelectItem();
                                Parse.CopyObject(xes[i], ref obj);
                                obj.Data4 = xes[i].Data4.ToString("dd/MM/yyyy");
                                kh.Xes.Add(obj);
                            }
                        }
                        return kh;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString, int loaiNV)
        {
            using (var db = new HMSEntities(connectString))
            {
                //if (loaiNV != 0)
                //  return db.H_KhachHang.Where(x => !x.IsDeleted && !x.H_LoaiNhanVien.IsDeleted && x.LoaiNVId == loaiNV).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code }).ToList();
                return db.H_KhachHang.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code }).ToList();
            }
        }

        public ResponseModel InsertOrUpdate(string connectString, KhachHangModel model)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (!CheckExists(model, db))
                {
                    H_KhachHang nv;
                    if (model.Id == 0)
                    {
                        //var last = db.H_KhachHang.OrderByDescending(x => x.Index).FirstOrDefault();
                        //int _index = 1;
                        //if (last != null)
                        //    _index = last.Index + 1;
                        //string code = "KH-" + DateTime.Now.ToString("ddMMyy") + "-" + getNumber(_index);
                        nv = new H_KhachHang()
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
                        //if (!string.IsNullOrEmpty(model.TaiKhoan) && !string.IsNullOrEmpty(model.MatKhau))
                        //{
                        //    H_Account acc = new H_Account() { AccName = model.TaiKhoan, Password = GlobalFunction.EncryptMD5(model.MatKhau) };
                        //    acc.H_KhachHang = nv;
                        //    nv.H_Account = new List<H_Account>();
                        //    nv.H_Account.Add(acc);
                        //}
                        db.H_KhachHang.Add(nv);
                    }
                    else
                    {
                        nv = db.H_KhachHang.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                        if (nv != null)
                        {
                            nv.Code = model.Ma;
                            nv.Name = model.Ten;
                            nv.Gender = model.GTinh;
                            nv.Birthday = model.NSinh;
                            nv.Address = model.DChi;
                            nv.Phone = model.DThoai;
                            nv.Note = model.Note;
                            nv.City = model.TPho;
                            nv.District = model.Huyen;
                            nv.Wards = model.Phuong;
                            nv.JobId = model.JobId;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.sms = "Khách hàng đã bị xóa hoặc không tồn tại trong hệ thống.!";
                        }
                    }
                    if (result.IsSuccess)
                    {
                        result.Data = nv.Id;
                        db.SaveChanges();
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.sms = "Mã này đã tồn tại trong hệ thống. Vui lòng chọn lại mã khác.";
                }
                return result;
            }
        }

        public bool Delete(string connectString, int Id)
        {
            using (var db = new HMSEntities(connectString))
            {
                var obj = db.H_KhachHang.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CheckExists(KhachHangModel KhachHang, HMSEntities db)
        {
            H_KhachHang obj = null;
            if (!string.IsNullOrEmpty(KhachHang.Ma))
                obj = db.H_KhachHang.FirstOrDefault(x => !x.IsDeleted && x.Id != KhachHang.Id && x.Code.Trim().ToUpper().Equals(KhachHang.Ma.Trim().ToUpper()));
            return obj != null ? true : false;
        }

        public string getNumber(int index)
        {
            string num = "00000";
            if (index < 10)
                num = "0000" + index;
            else if (index > 10 && index < 100)
                num = "000" + index;
            else if (index > 100 && index < 1000)
                num = "00" + index;
            else if (index > 1000 && index < 10000)
                num = "0" + index;
            else
                num = index.ToString();
            return num;
        }
    }
}
