﻿using HeadManagementSystem.Filter;
using System.Web;
using System.Web.Mvc;

namespace HeadManagementSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HMSAuthorize());
        }
    }
}
