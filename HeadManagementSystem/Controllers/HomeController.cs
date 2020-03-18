using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HeadManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData  qRData = qRCodeGenerator.CreateQrCode("nguyễn hoàng hải", QRCodeGenerator.ECCLevel.M );
            QRCode qRCode = new QRCode(qRData);
            using (Bitmap bitmap = qRCode.GetGraphic(250))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream, ImageFormat.Png);                     
                    ViewBag.ImageUrl = "data:image/png;base64," + Convert.ToBase64String( memoryStream.ToArray());
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}