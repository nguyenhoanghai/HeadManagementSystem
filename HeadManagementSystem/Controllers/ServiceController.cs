using HeadManagementSystem.Helper;
using HeadManagementSystem.Models;
using HMS.Data.BLL;
using HMS.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Http;

namespace HeadManagementSystem.Controllers
{
    public class ServiceController : ApiController
    {
        public string connectString = App_Global.AppGlobal.Connectionstring;
        [HttpGet]
        public KhachHangModel checkKH(string makh)
        {
            var kh = BLLKhachHang.Instance.FindSoKhungOrSoMayOrBSo(connectString, makh);
            if (kh != null)
            {
                kh.strNgayMua = kh.NgayMua.ToString("dd-MM-yyyy HH-mm");
                kh.strNgayTao = kh.NgayTao.ToString("dd-MM-yyyy");
                return kh;
            }
            else
                return new KhachHangModel();
        }

        public List<ModelSelectItem> getUsers()
        {
            return BLLUser.Instance.GetLookUp(connectString, 0);
        }


        public List<ModelSelectItem> getModels()
        {
            return BLLModel.Instance.GetLookUp(connectString);
        }


        public List<ModelSelectItem> getWorkTypes()
        {
            return BLLWorkType.Instance.GetLookUp(connectString);
        }

        public CollectionModel getusers_models_worktypes(bool isSell, int khId)
        {
            var collec = new CollectionModel();
            int index = 0;
            if (isSell)
            {
                index = BLLSellReceipt.Instance.GetLastInDay(connectString, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));
                collec.SellNumber = "BH-" + DateTime.Now.ToString("ddMMyy") + "-" + Helper.GPRO_Helper.Instance.getNumber(index);
                collec.item1 = BLLUser.Instance.GetLookUp(connectString, 0);
            }
            else
            {
                index = BLLReceipt.Instance.GetLastInDay(connectString, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));
                collec.SerNumber = "DV-" + DateTime.Now.ToString("ddMMyy") + "-" + Helper.GPRO_Helper.Instance.getNumber(index);
                collec.item4 = BLLKhachHang.Instance.GetAllbikes(connectString, khId);
            }

            collec.item2 = BLLModel.Instance.GetLookUp(connectString);
            collec.item3 = BLLWorkType.Instance.GetLookUp(connectString);
            collec.index = index;
            return collec;
        }

        [HttpGet]
        public ResponseModel createSellReceipt(string json)
        {
            SellReceiptModel model = JsonConvert.DeserializeObject<SellReceiptModel>(json);
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime dt = DateTime.ParseExact(model.strNgay + " 00:00:00", "dd-MM-yyyy HH:mm:ss", provider);
            model.Ngay = dt;
            model.Id = model.KHId;
            return BLLSellReceipt.Instance.Insert(connectString, model);
        }

        [HttpGet]
        public ResponseModel createServiceReceipt(string json)
        {
            PhieuModel model = JsonConvert.DeserializeObject<PhieuModel>(json);
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime dt = DateTime.ParseExact(model.strNgay + " 00:00:00", "dd-MM-yyyy HH:mm:ss", provider);
            model.Ngay = dt;
            var rs = BLLReceipt.Instance.Insert(connectString, model);
            if (rs.IsSuccess && App_Global.AppGlobal.UseQMS == "1")
            {
                // dung qms
                //gui yeu cau cap phieu
                var qmsResult = QMSAPI.Instance.InPhieu(model.SoPhieu, model.QMSServiceId, "", "");
                if (qmsResult.IsSuccess)
                {
                    //data => stt
                    //data_1 => ten dv
                    rs.Data = qmsResult.Data;
                    rs.Data_1 = qmsResult.Data_1;
                    rs.Data_2 = "Ngay: " + DateTime.Now.ToString("dd/MM/yyyy") + "    Gio: " + DateTime.Now.ToString("HH/mm");
                }
            }
            return rs;
        }

        public CollectionModel gethistories(int khId)
        {
            var collec = new CollectionModel();
            collec.item1 = BLLKhachHang.Instance.GetAllDVs(connectString, khId);
            collec.item2 = BLLKhachHang.Instance.GetAllbikes(connectString, khId);
            return collec;
        }

        [HttpGet]
        public PhieuEditModel getReceiptDe(int receiptId)
        {
            var p = BLLReceipt.Instance.Get(connectString, receiptId);
            if (p != null)
            {
                p.strNgay = p.Ngay.ToString("dd-MM-yyyy HH:mm");
                p.strNgayDong = "";
                if (p.NgayDong.HasValue)
                    p.strNgayDong = p.NgayDong.Value.ToString("dd-MM-yyyy HH:mm");
            }
            return p;
        }
    }
}
