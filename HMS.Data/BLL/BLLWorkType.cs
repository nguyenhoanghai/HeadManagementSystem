using HMS.Data.Model; 
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLWorkType
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLWorkType _Instance;  //volatile =>  tranh dung thread
        public static BLLWorkType Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLWorkType();

                return _Instance;
            }
        }
        private BLLWorkType() { }
        #endregion
        public List<WorkTypeModel> Gets(string connectString)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                { 
                    return db.H_WorkType.Where(x => !x.IsDeleted).Select(x => new WorkTypeModel() { Id = x.Id, Code = x.Code, Name = x.Name }).ToList();
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
                return db.H_WorkType.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Code = x.Code, Name = x.Name }).ToList();
            }
        }

        public ResponseModel InsertOrUpdate(string connectString, H_WorkType WorkType)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (!CheckExists(WorkType,db))
                {
                    if (WorkType.Id == 0)
                        db.H_WorkType.Add(WorkType);
                    else
                    {
                        var found = db.H_WorkType.FirstOrDefault(x => !x.IsDeleted && x.Id == WorkType.Id);
                        if (found != null)
                        {
                            found.Code = WorkType.Code;
                            found.Name = WorkType.Name; 
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
                var obj = db.H_WorkType.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(H_WorkType WorkType, HMSEntities db)
        {
            H_WorkType obj = null;
            if (!string.IsNullOrEmpty(WorkType.Code))
                obj = db.H_WorkType.FirstOrDefault(x => !x.IsDeleted && x.Id != WorkType.Id && x.Code.Trim().ToUpper().Equals(WorkType.Code.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
