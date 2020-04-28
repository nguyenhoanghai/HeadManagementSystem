using HeadManagementSystem.Constant.Authentication;
using HeadManagementSystem.Filter;
using HMS.Data.BLL;
using HMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HeadManagementSystem.Controllers
{
    public class PhuTungController : BaseController
    {
        [HMSAuthorize(Roles = UserRoleConstant.Member+","+UserRoleConstant.Admin)]
        public ActionResult Index()
        { 
            return View();
        }

        [HttpPost]
        public JsonResult Gets()
        { 
            return Json(BLLPhuTung.Instance.Gets(App_Global.AppGlobal.Connectionstring) );
        }
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            var result = BLLPhuTung.Instance.Delete(App_Global.AppGlobal.Connectionstring, Id);
            if (result)
                return Json(new { success = true, responseText = "Save success." }, JsonRequestBehavior.AllowGet);
            return Json(new { success = false, responseText = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(PhuTungModel model)
        {
            var rs = BLLPhuTung.Instance.InsertOrUpdate(App_Global.AppGlobal.Connectionstring, model);
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
                        code= BLLNhapPT.Instance.InsertFromExcel(fileUp.InputStream, "", App_Global.AppGlobal.Connectionstring);                         
                    }
                }
                catch (Exception ex)
                { }
            }
            return Json(code);
          
           
        }
    }
}