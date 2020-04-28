using HMS.Data.Model; 
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLJob
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLJob _Instance;  //volatile =>  tranh dung thread
        public static BLLJob Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLJob();

                return _Instance;
            }
        }
        private BLLJob() { }
        #endregion

        public List<JobModel> Gets(string connectString)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                { 
                    return db.H_NgheNghiep.Where(x => !x.IsDeleted).Select(x => new JobModel() { Id = x.Id, Note = x.Note, Name = x.Code }).ToList();
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
                return db.H_NgheNghiep.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Code = x.Code, Name = x.Note }).ToList();
            }
        }

        public ResponseModel InsertOrUpdate(string connectString, H_NgheNghiep Job)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (!CheckExists(Job,db))
                {
                    if (Job.Id == 0)
                        db.H_NgheNghiep.Add(Job);
                    else
                    {
                        var found = db.H_NgheNghiep.FirstOrDefault(x => !x.IsDeleted && x.Id == Job.Id);
                        if (found != null)
                        {
                            found.Code = Job.Code;
                            found.Note = Job.Note; 
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.sms = "Nghề nghiệp này đã bị xóa hoặc không tồn tại trong hệ thống.!";
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
                var obj = db.H_NgheNghiep.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(H_NgheNghiep Job, HMSEntities db)
        {
            H_NgheNghiep obj = null;
            if (!string.IsNullOrEmpty(Job.Code))
                obj = db.H_NgheNghiep.FirstOrDefault(x => !x.IsDeleted && x.Id != Job.Id && x.Code.Trim().ToUpper().Equals(Job.Code.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
