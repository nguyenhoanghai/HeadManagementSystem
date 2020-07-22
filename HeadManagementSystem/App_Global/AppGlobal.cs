using System;
using System.Configuration;

namespace HeadManagementSystem.App_Global
{
    public class AppGlobal
    {
        private static string _connectionstring;
        public static string Connectionstring
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionstring))
                {
                    _connectionstring = Helper.GPRO_Helper.Instance.GetEntityConnectString();
                }
                return _connectionstring;
            }
        }

        public static string ProductCode = "GPRO_HMS";
        public static string QMSUrl
        {
            get { return ConfigurationManager.AppSettings["QMSUrl"]; }
        }

        public static string UseQMS
        {
            get { return (ConfigurationManager.AppSettings["UseQMS"] != null ? ConfigurationManager.AppSettings["UseQMS"].ToString()  : "0"); }
        }
    }
}