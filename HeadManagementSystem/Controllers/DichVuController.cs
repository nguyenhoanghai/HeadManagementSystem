using HeadManagementSystem.Constant.Authentication;
using HeadManagementSystem.Filter;
using HMS.Data.BLL;
using HMS.Data.Model;
using System;
using System.Globalization;
using System.Web.Mvc;

namespace HeadManagementSystem.Controllers
{
    public class DichVuController : Controller
    {
        [HMSAuthorize(Roles = UserRoleConstant.Member+","+UserRoleConstant.Admin)] 
        public ActionResult Index()
        {
            ViewBag.loaiCVs = BLLWorkType.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring);
            ViewBag.congViecs = BLLWork.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring);
            return View();
        }

        [HttpPost]
        public JsonResult Gets()
        {
            return Json(BLLDichVu.Instance.Gets(App_Global.AppGlobal.Connectionstring));
        }

        [HMSAuthorize(Roles = UserRoleConstant.Member+","+UserRoleConstant.Admin)] 
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            var result = BLLDichVu.Instance.Delete(App_Global.AppGlobal.Connectionstring, Id);
            if (result)
                return Json(new { success = true, responseText = "Save success." }, JsonRequestBehavior.AllowGet);
            return Json(new { success = false, responseText = result }, JsonRequestBehavior.AllowGet);
        }

        [HMSAuthorize(Roles = UserRoleConstant.Member+","+UserRoleConstant.Admin)]
        [HttpPost]
        public JsonResult Save(DichVuModel model)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var newDate = DateTime.ParseExact("01/01/2018 " + model.strTGXL+":00", "dd/MM/yyyy HH:mm:ss", provider);
            model.TGXL = newDate;
            model.GiaBan = model.GiaMua;
            var rs = BLLDichVu.Instance.InsertOrUpdate(App_Global.AppGlobal.Connectionstring, model);
            return Json(rs);
        }
    }
}