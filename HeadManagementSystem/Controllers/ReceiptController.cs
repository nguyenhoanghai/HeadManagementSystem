using HeadManagementSystem.Models;
using HMS.Data.BLL;
using System.Web.Mvc;

namespace HeadManagementSystem.Controllers
{
    public class ReceiptController : Controller
    {
        // GET: Receipt
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(string ma)
        {

            ViewBag.Jobs = BLLJob.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring);
            ViewBag.Models = BLLModel.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring);
            DichVuModel dichVuModel = new DichVuModel();
            if (!string.IsNullOrEmpty(ma))
            {
                dichVuModel.Ma = ma;
                var kh = BLLKhachHang.Instance.GetByCode(App_Global.AppGlobal.Connectionstring, ma);
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
            }
            return View(dichVuModel);
        }

        [HttpPost]
        public ActionResult Create(DichVuModel model)
        {
            ViewBag.Jobs = BLLJob.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring);
            ViewBag.Models = BLLModel.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring);
            return View(model);
        }

        //public JsonResult Save()
        //{ 
        //    ViewBag.Models = BLLModel.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring);
        //    return View();
        //}
    }
}