using HeadManagementSystem.Constant.Authentication;
using HeadManagementSystem.Filter;
using HMS.Data;
using HMS.Data.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HeadManagementSystem.Controllers
{
    public class ModelController : Controller
    {
        [HMSAuthorize(Roles = UserRoleConstant.Member+","+UserRoleConstant.Admin)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Gets()
        {
            return Json(BLLModel.Instance.Gets(App_Global.AppGlobal.Connectionstring));
        }

        [HMSAuthorize(Roles = UserRoleConstant.Member+","+UserRoleConstant.Admin)]
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            var result = BLLModel.Instance.Delete(App_Global.AppGlobal.Connectionstring,Id);
            if (result )
                return Json(new { success = true, responseText = "Save success." }, JsonRequestBehavior.AllowGet);
            return Json(new { success = false, responseText = result }, JsonRequestBehavior.AllowGet);
        }

        [HMSAuthorize(Roles = UserRoleConstant.Member+","+UserRoleConstant.Admin)]
        [HttpPost] 
        public JsonResult Save(H_Model model)
        {
            var rs = BLLModel.Instance.InsertOrUpdate(App_Global.AppGlobal.Connectionstring,model);
            return Json(rs);
        }
    }
}