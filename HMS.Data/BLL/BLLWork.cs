using HMS.Data.Model; 
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLWork
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLWork _Instance;  //volatile =>  tranh dung thread
        public static BLLWork Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLWork();

                return _Instance;
            }
        }
        private BLLWork() { }
        #endregion
        public List<WorkModel> Gets(string connectString)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                { 
                    return db.H_Work.Where(x => !x.IsDeleted).Select(x => new WorkModel() { Id = x.Id, Code = x.Code, Name = x.Name }).ToList();
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
                return db.H_Work.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Code = x.Code, Name = x.Name }).ToList();
            }
        }

        public ResponseModel InsertOrUpdate(string connectString, H_Work Work)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (!CheckExists(Work,db))
                {
                    if (Work.Id == 0)
                        db.H_Work.Add(Work);
                    else
                    {
                        var found = db.H_Work.FirstOrDefault(x => !x.IsDeleted && x.Id == Work.Id);
                        if (found != null)
                        {
                            found.Code = Work.Code;
                            found.Name = Work.Name; 
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.sms = "Loại công việc xe đã bị xóa hoặc không tồn tại trong hệ thống.!";
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
                var obj = db.H_Work.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(H_Work Work, HMSEntities db)
        {
            H_Work obj = null;
            if (!string.IsNullOrEmpty(Work.Code))
                obj = db.H_Work.FirstOrDefault(x => !x.IsDeleted && x.Id != Work.Id && x.Code.Trim().ToUpper().Equals(Work.Code.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
