using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HeadManagementSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}"); 
            routes.MapRoute(
            name: "lg",
            url: "ketnoicsdl",
            defaults: new { controller = "sqlconnect", action = "index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Receipt", action = "Phieumo", id = UrlParameter.Optional }
            );
        }
    }
}
