using HeadManagementSystem.App_Global;
using QMS_System.Data.Model;
using System;
using System.Net.Http;

namespace HeadManagementSystem.Helper
{
    public class QMSAPI
    {
        #region Constructor
        private static HttpClient QMSClient;
        private static object key = new object();
        private static volatile QMSAPI _Instance;
        public static QMSAPI Instance
        {
            get
            {
                if (QMSAPI._Instance == null)
                {
                    lock (QMSAPI.key)
                    {
                        QMSAPI._Instance = new QMSAPI();
                        QMSAPI.QMSClient = new HttpClient();
                        QMSAPI.QMSClient.BaseAddress = new Uri(AppGlobal.QMSUrl);
                    }
                }
                return QMSAPI._Instance;
            }
        }
        private QMSAPI()
        {
        }
        #endregion

        /// <summary>
        /// gọi QMS yêu cầu cấp số & in phiếu
        /// </summary>
        /// <param name="maphieudichvu">mã số phiếu dịch vụ của cửa hàng</param>
        /// <param name="madichvu">mã dịch vụ bên QMS</param>
        /// <param name="macongviec">danh sách các id công việc chọn để tính thời gian xử lý</param>
        /// <param name="loaixe">xe máy | xe ga</param>
        /// <returns></returns>
        public ResponseBase InPhieu(string maphieudichvu, int madichvu, string macongviec, string loaixe)
        {
            try
            {
                string param = string.Empty;
                HttpResponseMessage resp = QMSClient.GetAsync(@"api/serviceapi/PrintTicket1?maphieudichvu=" + maphieudichvu + "&madichvu=" + madichvu + "&macongviec=" + macongviec + "&loaixe=" + loaixe).Result;
                resp.EnsureSuccessStatusCode();
                return resp.Content.ReadAsAsync<ResponseBase>().Result;
            }
            catch (Exception)
            { }
            return null;
        }
    }
}