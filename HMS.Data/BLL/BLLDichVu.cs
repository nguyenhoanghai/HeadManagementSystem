using HMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLDichVu
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLDichVu _Instance;  //volatile =>  tranh dung thread
        public static BLLDichVu Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLDichVu();

                return _Instance;
            }
        }
        private BLLDichVu() { }
        #endregion
        public List<DichVuModel> Gets(string connectString)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    return db.H_DichVu.Where(x => !x.IsDeleted).Select(x => new DichVuModel()
                    {
                        Id = x.Id,
                        LoaiCV = x.H_WorkType.Name,
                        MaCV = x.H_Work.Code,
                        TenCV = x.H_Work.Name,
                        LoaiCVId = x.WorkTypeId,
                        CVId = x.WorkId,
                        TGXL = x.TimeProcess,
                        GiaBan = x.Price_Out,
                        GiaMua = x.Price_In,
                        Note = x.Note
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<ModelSelectItem> GetLookUp(string connectString, int wtypeId)
        {
            using (var db = new HMSEntities(connectString))
            {
                if (wtypeId > 0)
                    return (from x in db.H_DichVu where !x.IsDeleted && x.WorkTypeId == wtypeId select new ModelSelectItem() { Id = x.Id, Code = x.H_Work.Code, Name = x.H_Work.Name, Data = x.WorkId, Record = x.WorkTypeId, _double = x.Price_In, _double1 = x.Price_Out }).ToList();
                return (from x in db.H_DichVu where !x.IsDeleted select new ModelSelectItem() { Id = x.Id, Code = x.H_Work.Code, Name = x.H_Work.Name, Data = x.WorkId, Record = x.WorkTypeId, _double = x.Price_In, _double1 = x.Price_Out }).ToList();
            }
        }

        public ResponseModel InsertOrUpdate(string connectString, DichVuModel model)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (!CheckExists(model, db))
                {
                    H_DichVu DichVu = null;
                    if (model.Id == 0)
                    {
                        DichVu = new H_DichVu()
                        {
                            WorkId = model.CVId,
                            WorkTypeId = model.LoaiCVId,
                            TimeProcess = model.TGXL,
                            Price_Out = model.GiaBan,
                            Price_In = model.GiaMua,
                            Note = model.Note
                        };
                        db.H_DichVu.Add(DichVu);
                    }
                    else
                    {
                        var found = db.H_DichVu.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                        if (found != null)
                        {
                            found.WorkId = model.CVId;
                            found.WorkTypeId = model.LoaiCVId;
                            found.TimeProcess = model.TGXL;
                            found.Price_In = model.GiaMua;
                            found.Price_Out = model.GiaBan;
                            found.Note = model.Note;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.sms = "Công việc đã bị xóa hoặc không tồn tại trong hệ thống.!";
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
                var obj = db.H_DichVu.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists(DichVuModel DichVu, HMSEntities db)
        {
            H_DichVu obj = db.H_DichVu.FirstOrDefault(x => !x.IsDeleted && x.Id != DichVu.Id && x.WorkId == DichVu.CVId && x.WorkTypeId == DichVu.LoaiCVId);
            return obj != null ? true : false;
        }
    }
}
