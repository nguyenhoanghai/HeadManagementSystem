using GPRO.Ultilities;
using HMS.Data.BLL;
using HMS.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HeadManagementSystem.Controllers
{
    public class ReceiptController : Controller
    {
        // GET: Receipt
        public ActionResult PhieuDong()
        {
            return View(BLLReceipt.Instance.PhieuDongs(App_Global.AppGlobal.Connectionstring));
        }

        public ActionResult PhieuMo()
        {
            return View(BLLReceipt.Instance.PhieuMos(App_Global.AppGlobal.Connectionstring));
        }

        #region Create
        public ActionResult Create(string ma)
        {
            string connnect = App_Global.AppGlobal.Connectionstring;
            ViewBag.Jobs = BLLJob.Instance.GetLookUp(connnect);
            ViewBag.Models = BLLModel.Instance.GetLookUp(connnect);
            ViewBag.LoaiXes = BLLWorkType.Instance.GetLookUp(connnect);
            var dichVuModel = new Models.DichVuModel();
            dichVuModel.Ngay = DateTime.Now;
            int index = BLLReceipt.Instance.GetLastInDay(connnect, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));
            dichVuModel.SoPhieu = "DV-" + DateTime.Now.ToString("ddMMyy") + "-" + Helper.GPRO_Helper.Instance.getNumber(index);
            dichVuModel.Index = index;
            dichVuModel.Ma = ma;
            if (!string.IsNullOrEmpty(ma))
            {               
                var kh = BLLKhachHang.Instance.GetByCode(connnect, ma);
                if (kh != null)
                {
                    dichVuModel.Id = kh.Id;
                    dichVuModel.Ten = kh.Ten;
                    dichVuModel.GTinh = kh.GTinh;
                    dichVuModel.NSinh = kh.NSinh;
                    dichVuModel.DThoai = kh.DThoai;
                    dichVuModel.DChi = kh.DChi;
                    dichVuModel.TPho = kh.TPho;
                    dichVuModel.Huyen = kh.Huyen;
                    dichVuModel.Phuong = kh.Phuong;
                    dichVuModel.Note = kh.Note;
                    dichVuModel.JobId = kh.JobId;
                    dichVuModel.Xes = kh.Xes; 
                }
                else
                    ViewBag.alert = "Không tìm thấy thông tin khách hàng trong hệ thống.";
            }
            return View(dichVuModel);
        }

        [HttpPost]
        public ActionResult Create(Models.DichVuModel model)
        {
            string connnect = App_Global.AppGlobal.Connectionstring;
            ViewBag.Jobs = BLLJob.Instance.GetLookUp(connnect);
            ViewBag.Models = BLLModel.Instance.GetLookUp(connnect);
            ViewBag.LoaiXes = BLLWorkType.Instance.GetLookUp(connnect);
            var obj = new PhieuModel();
            Parse.CopyObject(model, ref obj);
            var result = BLLReceipt.Instance.Insert(connnect, obj);
            return RedirectToAction("phieumo");
        }
        #endregion

        #region Edit
        public ActionResult Edit(int id)
        {
            var _model = BLLReceipt.Instance.Get(App_Global.AppGlobal.Connectionstring, id);
            ViewBag.NhanViens = BLLUser.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring, 0);
            ViewBag.Works = JsonConvert.SerializeObject(BLLDichVu.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring, _model.LoaiXeId));
            ViewBag.Phutungs = JsonConvert.SerializeObject(BLLPhuTung.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring));
             return View(_model);
        }

        [HttpPost]
        public ActionResult Edit(PhieuEditModel model)
        {
            model.DichVus = JsonConvert.DeserializeObject<List<PhieuDichVuModel>>(model.jsonDichVus);
            model.PhuTungs = JsonConvert.DeserializeObject<List<PhieuPhuTungModel>>(model.jsonPhuTungs);
            var rs = BLLReceipt.Instance.Edit(App_Global.AppGlobal.Connectionstring, model);
            ViewBag.NhanViens = BLLUser.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring, 0);
            ViewBag.Works = JsonConvert.SerializeObject(BLLDichVu.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring, model.LoaiXeId));
            ViewBag.Phutungs = JsonConvert.SerializeObject(BLLPhuTung.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring));
            return RedirectToAction("phieumo");
        }

        #endregion

        public ActionResult Detail(int id)
        {
            var _model = BLLReceipt.Instance.Get(App_Global.AppGlobal.Connectionstring, id);
            _model.TongCong = _model.DichVus.Sum(x => x.GiaBan - ((x.GiaBan * x.CKhau) / 100));
            _model.TongPT = _model.PhuTungs.Sum(x => x.GiaBan - ((x.GiaBan * x.CKhau) / 100));
              return View(_model);
        }

        public ActionResult Close(int Id)
        {
            BLLReceipt.Instance.Close(App_Global.AppGlobal.Connectionstring, Id);
            return RedirectToAction("phieumo");
        }
    }
}