using HeadManagementSystem.Constant.Authentication;
using HeadManagementSystem.Filter;
using HMS.Data.BLL;
using HMS.Data.Model;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;

namespace HeadManagementSystem.Controllers
{
    public class KhachHangController : Controller
    {
        [HMSAuthorize(Roles = UserRoleConstant.Member + "," + UserRoleConstant.Admin)]
        public ActionResult Index()
        {
            ViewBag.Jobs = BLLJob.Instance.GetLookUp(App_Global.AppGlobal.Connectionstring);
            return View();
        }

        [HMSAuthorize(Roles = UserRoleConstant.Member + "," + UserRoleConstant.Admin)]
        [HttpPost]
        public JsonResult Gets()
        {
            return Json(BLLKhachHang.Instance.Gets(App_Global.AppGlobal.Connectionstring));
        }

        [HMSAuthorize(Roles = UserRoleConstant.Member + "," + UserRoleConstant.Admin)]
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            var result = BLLKhachHang.Instance.Delete(App_Global.AppGlobal.Connectionstring, Id);
            if (result)
                return Json(new { success = true, responseText = "Save success." }, JsonRequestBehavior.AllowGet);
            return Json(new { success = false, responseText = result }, JsonRequestBehavior.AllowGet);
        }

        [HMSAuthorize(Roles = UserRoleConstant.Member + "," + UserRoleConstant.Admin)]
        [HttpPost]
        public JsonResult Save(KhachHangModel model)
        {
            var rs = BLLKhachHang.Instance.InsertOrUpdate(App_Global.AppGlobal.Connectionstring, model);
            return Json(rs);
        }

        [HMSAuthorize(Roles = UserRoleConstant.Member + "," + UserRoleConstant.Admin)]
        [HttpPost]
        public JsonResult Get(string data)
        {
            var kh = BLLKhachHang.Instance.GetByCode(App_Global.AppGlobal.Connectionstring, data);
            if (kh != null)
                return Json(kh);
            return Json("");
        }

        public ActionResult Detail(int id)
        {
            string conn = App_Global.AppGlobal.Connectionstring;
            var kh = BLLKhachHang.Instance.Get(conn, id);
            if (kh == null)
                return RedirectToAction("Page404", "Error");
            ViewBag.DichVus = BLLReceipt.Instance.KhachHangPhieus(conn, id);
            ViewBag.Muas = BLLSellReceipt.Instance.KhachHangPhieus(conn, id);

            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRData = qRCodeGenerator.CreateQrCode(kh.Ma, QRCodeGenerator.ECCLevel.M);
            QRCode qRCode = new QRCode(qRData);
            using (Bitmap bitmap = qRCode.GetGraphic(250))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream, ImageFormat.Png);
                    ViewBag.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            return View(kh);
        }

    }
}