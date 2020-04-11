using HMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLModel
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLModel _Instance;  //volatile =>  tranh dung thread
        public static BLLModel Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLModel();

                return _Instance;
            }
        }
        private BLLModel() { }
        #endregion
        public List<XeModel> Gets(string connectString)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                { 
                    return db.H_Model.Where(x => !x.IsDeleted).Select(x => new XeModel() { Id = x.Id, Name = x.Code, Note = x.Note }).ToList();
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
                return db.H_Model.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Name = x.Code }).ToList();
            }
        }

        public ResponseModel InsertOrUpdate(string connectString, H_Model model)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (!CheckExists(model,db))
                {
                    if (model.Id == 0)
                        db.H_Model.Add(model);
                    else
                    {
                        var found = db.H_Model.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                        if (found != null)
                        {
                            found.Code = model.Code;
                            found.Note = model.Note;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.sms = "Model xe đã bị xóa hoặc không tồn tại trong hệ thống.!";
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
                var obj = db.H_Model.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(H_Model model, HMSEntities db)
        {
            H_Model obj = null;
            if (!string.IsNullOrEmpty(model.Code))
                obj = db.H_Model.FirstOrDefault(x => !x.IsDeleted && x.Id != model.Id && x.Code.Trim().ToUpper().Equals(model.Code.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
