using HeadManagementSystem.Constant.Authentication;
using HeadManagementSystem.Filter;
using HMS.Data;
using HMS.Data.BLL;
using HMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HeadManagementSystem.Controllers
{
    public class UserController : Controller
    {
        [HMSAuthorize(Roles = UserRoleConstant.Admin)]
        public ActionResult Index()
        {
            ViewBag.loaiNV = BLLUserType.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring);
            return View();
        }

        [HttpPost]
        public JsonResult Gets()
        {
            var users = BLLUser.Instance.Gets(App_Global.AppGlobal.Connectionstring);
            return Json(users);
        }
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            var result = BLLUser.Instance.Delete(App_Global.AppGlobal.Connectionstring, Id);
            if (result)
                return Json(new { success = true, responseText = "Save success." }, JsonRequestBehavior.AllowGet);
            return Json(new { success = false, responseText = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(NhanVienModel model)
        {
            var rs = BLLUser.Instance.InsertOrUpdate(App_Global.AppGlobal.Connectionstring, model);
            return Json(rs);
        }

        public JsonResult InsertExcel()
        {
            int code = 0;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var fileUp = System.Web.HttpContext.Current.Request.Files["file"];
                try
                {
                    if (fileUp != null)
                    {
                        code = BLLUser.Instance.InsertFromExcel(fileUp.InputStream, "", App_Global.AppGlobal.Connectionstring);
                    }
                }
                catch (Exception ex)
                { }
            }
            return Json(code);
        }
    }
}