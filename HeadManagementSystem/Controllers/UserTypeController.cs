using HeadManagementSystem.Constant.Authentication;
using HeadManagementSystem.Filter;
using HMS.Data;
using HMS.Data.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HeadManagementSystem.Controllers
{
    public class UserTypeController : Controller
    {
        [HMSAuthorize(Roles = UserRoleConstant.Admin)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Gets()
        {
            return Json(BLLUserType.Instance.Gets(App_Global.AppGlobal.Connectionstring));
        }
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            var result = BLLUserType.Instance.Delete(App_Global.AppGlobal.Connectionstring, Id);
            if (result)
                return Json(new { success = true, responseText = "Save success." }, JsonRequestBehavior.AllowGet);
            return Json(new { success = false, responseText = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(H_LoaiNhanVien model)
        {
            var rs = BLLUserType.Instance.InsertOrUpdate(App_Global.AppGlobal.Connectionstring, model);
            return Json(rs);
        }
    }
}