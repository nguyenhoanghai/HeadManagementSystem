using GPRO.Ultilities;
using HMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLUser
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLUser _Instance;  //volatile =>  tranh dung thread
        public static BLLUser Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLUser();

                return _Instance;
            }
        }
        private BLLUser() { }
        #endregion

        public List<NhanVienModel> Gets(string connectString)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    var userIds = db.H_Account.Where(x => !x.IsDeleted && x.IsAdmin).Select(x => x.NhanVienId).ToList();
                    return db.H_NhanVien.Where(x => !x.IsDeleted && !userIds.Contains(x.Id)).Select(x => new NhanVienModel()
                    {
                        Id = x.Id,
                        Ma = x.Code,
                        Ten = x.Name,
                        DienThoai = x.Phone,
                        DiaChi = x.Address,
                        LoaiNV = x.LoaiNVId,
                        strLoaiNV = x.H_LoaiNhanVien.Code,
                        Note = x.Note
                    }).ToList();
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
                if (loaiNV != 0)
                    return db.H_NhanVien.Where(x => !x.IsDeleted && !x.H_LoaiNhanVien.IsDeleted && x.LoaiNVId == loaiNV).Select(x => new ModelSelectItem() { Id = x.Id, Code = x.Code, Name = x.Name }).ToList();
                return db.H_NhanVien.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Code = x.Code, Name = x.Name }).ToList();
            }
        }

        public ResponseModel InsertOrUpdate(string connectString, NhanVienModel model)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (!CheckExists(model, db))
                {
                    H_NhanVien nv;
                    if (model.Id == 0)
                    {
                        var last = db.H_NhanVien.OrderByDescending(x => x.Index).FirstOrDefault();
                        string code = "NV-" + DateTime.Now.ToString("ddMMyy") + "-"+getNumber(last.Index+1);
                        nv = new H_NhanVien()
                        {
                            Code = code,
                            Name = model.Ten,
                            Phone = model.DienThoai,
                            Address = model.DiaChi,
                            Note = model.Note,
                            LoaiNVId = model.LoaiNV,
                            Index = (last.Index + 1)
                        };
                        if (!string.IsNullOrEmpty(model.TaiKhoan) && !string.IsNullOrEmpty(model.MatKhau))
                        {
                            H_Account acc = new H_Account() { AccName = model.TaiKhoan, Password = GlobalFunction.EncryptMD5(model.MatKhau) };
                            acc.H_NhanVien = nv;
                            nv.H_Account = new List<H_Account>();
                            nv.H_Account.Add(acc);
                        }
                        db.H_NhanVien.Add(nv);
                    }
                    else
                    {
                        nv = db.H_NhanVien.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                        if (nv != null)
                        { 
                            nv.Name = model.Ten;
                            nv.Address = model.DiaChi;
                            nv.Phone = model.DienThoai;
                            nv.LoaiNVId = model.LoaiNV;
                            nv.Note = model.Note;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.sms = "Nhân viên đã bị xóa hoặc không tồn tại trong hệ thống.!";
                        }
                    }
                    if (result.IsSuccess)
                        db.SaveChanges();
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
                var obj = db.H_NhanVien.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private bool CheckExists(NhanVienModel User, HMSEntities db)
        {
            H_NhanVien obj = null;
            if (!string.IsNullOrEmpty(User.Ma))
                obj = db.H_NhanVien.FirstOrDefault(x => !x.IsDeleted && x.Id != User.Id && x.Code.Trim().ToUpper().Equals(User.Ma.Trim().ToUpper()));
            return obj != null ? true : false;
        }

        public ResponseModel UpdateInfo(string connectString, NhanVienModel model, int accId)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                var acc = db.H_Account.FirstOrDefault(x => !x.IsDeleted && x.Id == accId);
                if (acc != null)
                {
                    var nv = db.H_NhanVien.FirstOrDefault(x => !x.IsDeleted && x.Id == acc.NhanVienId);
                    if (nv != null)
                    {
                        nv.Name = model.Ten;
                        nv.Address = model.DiaChi;
                        nv.Phone = model.DienThoai;
                        nv.Note = model.Note;
                        db.SaveChanges();
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.sms = "Nhân viên đã bị xóa hoặc không tồn tại trong hệ thống.!";
                    }
                }
                return result;
            }
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
