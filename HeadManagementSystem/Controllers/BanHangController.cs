using HMS.Data.BLL;
using HMS.Data.Model;
using System.Web.Mvc;

namespace HeadManagementSystem.Controllers
{
    public class BanHangController : Controller
    {

        public ActionResult Create()
        {
            ViewBag.Jobs = BLLJob.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring);
            ViewBag.Models = BLLModel.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring);
            ViewBag.Nhanviens = BLLUser.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring, 0);
            return View();
        }

        public JsonResult Save(SellReceiptModel model)
        {
            return Json(BLLSellReceipt.Instance.InsertOrUpdate(App_Global.AppGlobal.Connectionstring, model));
        }
    }
}