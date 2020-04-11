using HMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Data.BLL
{
    public class BLLPhuTung
    {
        #region constructor  
        static object key = new object();
        private static volatile BLLPhuTung _Instance;  //volatile =>  tranh dung thread
        public static BLLPhuTung Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLPhuTung();

                return _Instance;
            }
        }
        private BLLPhuTung() { }
        #endregion
        public List<PhuTungModel> Gets(string connectString)
        {
            using (var db = new HMSEntities(connectString))
            {
                try
                {
                    return db.H_PhuTung.Where(x => !x.IsDeleted).Select(x => new PhuTungModel()
                    {
                        Id = x.Id,
                        Ma = x.Code,
                        Ten = x.Name,
                        SoLuong = x.Quantities,
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

        public List<ModelSelectItem> GetLookUp(string connectString)
        {
            using (var db = new HMSEntities(connectString))
            {
                return db.H_PhuTung.Where(x => !x.IsDeleted).Select(x => new ModelSelectItem() { Id = x.Id, Code = x.Code, Name = x.Name, Data = x.Quantities, Data1 = x.Price_In, Data2 = x.Price_Out }).ToList();
            }
        }

        public ResponseModel InsertOrUpdate(string connectString, PhuTungModel model)
        {
            var result = new ResponseModel();
            result.IsSuccess = true;
            using (var db = new HMSEntities(connectString))
            {
                if (!CheckExists(model, db))
                {
                    H_PhuTung phutung = null;
                    if (model.Id == 0)
                    {
                        phutung = new H_PhuTung()
                        {
                            Code = model.Ma,
                            Name = model.Ten,
                            Quantities = model.SoLuong,
                            Price_Out = model.GiaBan,
                            Price_In = model.GiaMua,
                            Note = model.Note
                        };
                        db.H_PhuTung.Add(phutung);
                    }
                    else
                    {
                        var found = db.H_PhuTung.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                        if (found != null)
                        {
                            found.Code = model.Ma;
                            found.Name = model.Ten;
                            found.Quantities = model.SoLuong;
                            found.Price_In = model.GiaMua;
                            found.Price_Out = model.GiaBan;
                            found.Note = model.Note;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.sms = "Phụ tùnng xe đã bị xóa hoặc không tồn tại trong hệ thống.!";
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
                var obj = db.H_PhuTung.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        private bool CheckExists( PhuTungModel PhuTung, HMSEntities db)
        {
            H_PhuTung obj = null;
            if (!string.IsNullOrEmpty(PhuTung.Ma))
                obj = db.H_PhuTung.FirstOrDefault(x => !x.IsDeleted && x.Id != PhuTung.Id && x.Code.Trim().ToUpper().Equals(PhuTung.Ma.Trim().ToUpper()));
            return obj != null ? true : false;
        }
    }
}
