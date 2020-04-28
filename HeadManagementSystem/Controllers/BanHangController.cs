using HMS.Data.BLL;
using HMS.Data.Model;
using System;
using System.Web.Mvc;

namespace HeadManagementSystem.Controllers
{
    public class BanHangController : Controller
    {
        public ActionResult Index(DateTime? from, DateTime? to)
        {
            if (!from.HasValue && !to.HasValue)
            {
                var now = DateTime.Now;
                from = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                to = new DateTime(now.Year, now.Month, now.Day, 23, 59, 0);
            }
            else
                to = to.Value.AddHours(23);
            var objs = BLLSellReceipt.Instance.Gets(App_Global.AppGlobal.Connectionstring, from.Value, to.Value);
            ViewBag.info = from.Value.ToString("dd/MM/yyyy") + " đến " + to.Value.ToString("dd/MM/yyyy");
            return View(objs);
        }

        public ActionResult Create(string ma)
        {
            string connnect = App_Global.AppGlobal.Connectionstring;
            ViewBag.Jobs = BLLJob.Instance.GetLookUp(connnect);
            ViewBag.Models = BLLModel.Instance.GetLookUp(connnect);
            ViewBag.Nhanviens = BLLUser.Instance.GetLookUp(connnect, 0);
            ViewBag.LoaiXes = BLLWorkType.Instance.GetLookUp(connnect);
            var dichVuModel = new SellReceiptModel();
            dichVuModel.Ngay = DateTime.Now;
            int index = BLLSellReceipt.Instance.GetLastInDay(connnect, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));
            dichVuModel.SoPhieu = "BH-" + DateTime.Now.ToString("ddMMyy") + "-" + Helper.GPRO_Helper.Instance.getNumber(index);
            dichVuModel.Index = index;
            dichVuModel.MaKH = ma;
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
                }
                else
                    ViewBag.alert = "Không tìm thấy thông tin khách hàng trong hệ thống.";
            }
            return View(dichVuModel);
        }

        [HttpPost]
        public ActionResult Create(SellReceiptModel model)
        {
            string connnect = App_Global.AppGlobal.Connectionstring;
            ViewBag.Jobs = BLLJob.Instance.GetLookUp(connnect);
            ViewBag.Models = BLLModel.Instance.GetLookUp(connnect);
            ViewBag.LoaiXes = BLLWorkType.Instance.GetLookUp(connnect);
            var result = BLLSellReceipt.Instance.Insert(connnect, model);
            return RedirectToAction("index");
        }

        public ActionResult Edit(int id)
        {
            string connnect = App_Global.AppGlobal.Connectionstring;
            var phieu = BLLSellReceipt.Instance.Get(connnect, id);
            if (phieu == null)
                return RedirectToAction("Page404", "Error");
            ViewBag.Jobs = BLLJob.Instance.GetLookUp(connnect);
            ViewBag.Models = BLLModel.Instance.GetLookUp(connnect);
            ViewBag.LoaiXes = BLLWorkType.Instance.GetLookUp(connnect);
            ViewBag.Nhanviens = BLLUser.Instance.GetLookUp(connnect, 0);
            phieu.Id = id;
            return View(phieu);
        }

        [HttpPost]
        public ActionResult Edit(SellReceiptModel model)
        {
            string connnect = App_Global.AppGlobal.Connectionstring;
            ViewBag.Jobs = BLLJob.Instance.GetLookUp(connnect);
            ViewBag.Models = BLLModel.Instance.GetLookUp(connnect);
            ViewBag.LoaiXes = BLLWorkType.Instance.GetLookUp(connnect);
            var result = BLLSellReceipt.Instance.Edit(connnect, model);
            return RedirectToAction("index");
        }

        public JsonResult Save(SellReceiptModel model)
        {
            return Json(BLLSellReceipt.Instance.InsertOrUpdate(App_Global.AppGlobal.Connectionstring, model));
        }
    }
}