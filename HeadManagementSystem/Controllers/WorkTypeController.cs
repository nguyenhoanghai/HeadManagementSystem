﻿using HeadManagementSystem.Constant.Authentication;
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
    public class WorkTypeController : Controller
    {
        [HMSAuthorize(Roles = UserRoleConstant.Member+","+UserRoleConstant.Admin)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Gets()
        {
            return Json(BLLWorkType.Instance.Gets(App_Global.AppGlobal.Connectionstring));
        }

        [HMSAuthorize(Roles = UserRoleConstant.Member+","+UserRoleConstant.Admin)]
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            var result = BLLWorkType.Instance.Delete(App_Global.AppGlobal.Connectionstring, Id);
            if (result)
                return Json(new { success = true, responseText = "Save success." }, JsonRequestBehavior.AllowGet);
            return Json(new { success = false, responseText = result }, JsonRequestBehavior.AllowGet);
        }

        [HMSAuthorize(Roles = UserRoleConstant.Member+","+UserRoleConstant.Admin)]
        [HttpPost]
        public JsonResult Save(H_WorkType model)
        {
            var rs = BLLWorkType.Instance.InsertOrUpdate(App_Global.AppGlobal.Connectionstring, model);
            return Json(rs);
        }
    }
}