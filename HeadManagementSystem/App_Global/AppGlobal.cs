using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                    _connectionstring = Helper.GPRO_Helper.Instance.GetEntityConnectString() ;
                }
                return _connectionstring;
            }
        }
    }
}