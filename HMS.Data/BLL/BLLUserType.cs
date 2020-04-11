using HMS.Data.Model; 
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLUserType
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLUserType _Instance;  //volatile =>  tranh dung thread
        public static BLLUserType Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLUserType();

                return _Instance;
            }
        }
        private BLLUserType() { }
        #endregion
        public List<UserTypeModel> Gets(string connectString)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                { 
                    return db.H_LoaiNhanVien.Where(x => !x.IsDeleted).Select(x => new UserTypeModel() { Id = x.Id, Name = x.Code, Note = x.Note }).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
            using (var db = new HMSEntities(connectString))
            {
                return db.H_LoaiNhanVien.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code }).ToList();
            }
        }

        public ResponseModel InsertOrUpdate(string connectString, H_LoaiNhanVien UserType)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (!CheckExists(UserType,db))
                {
                    if (UserType.Id == 0)
                        db.H_LoaiNhanVien.Add(UserType);
                    else
                    {
                        var found = db.H_LoaiNhanVien.FirstOrDefault(x => !x.IsDeleted && x.Id == UserType.Id);
                        if (found != null)
                        {
                            found.Code = UserType.Code;
                            found.Note = UserType.Note;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.sms = "Loại nhân viên đã bị xóa hoặc không tồn tại trong hệ thống.!";
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
                var obj = db.H_LoaiNhanVien.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(H_LoaiNhanVien UserType, HMSEntities db)
        {
            H_LoaiNhanVien obj = null;
            if (!string.IsNullOrEmpty(UserType.Code))
                obj = db.H_LoaiNhanVien.FirstOrDefault(x => !x.IsDeleted && x.Id != UserType.Id && x.Code.Trim().ToUpper().Equals(UserType.Code.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
