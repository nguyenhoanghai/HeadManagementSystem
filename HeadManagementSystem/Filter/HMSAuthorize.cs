using HeadManagementSystem.Constant.Authentication;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace HeadManagementSystem.Filter
{
    public class HMSAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.Path.Trim().ToLower() == "/sqlconnect/index" ||
                httpContext.Request.Path.Trim().ToLower() == "/sqlconnect" ||
                httpContext.Request.Path.Trim() == "/SQLConnect/GetDatabases" ||
                httpContext.Request.Path.Trim().ToLower() == "/ketnoicsdl" ||
                httpContext.Request.Path.Trim() == "/SQLConnect/Save")
                return true;
            HttpCookie authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && !authTicket.Expired)
                    {
                        if (authTicket.Version == (int)FormAuthenticationTicketVersion.Admin ||
                            authTicket.Version == (int)FormAuthenticationTicketVersion.SuperAdmin)
                        {
                            var userData = JsonConvert.DeserializeObject<BaseLoginInformation>(authTicket.UserData);
                            HttpContext.Current.User = new UserPrincipal<BaseLoginInformation>(new FormsIdentity(authTicket), userData);
                        }
                        else if (authTicket.Version == (int)FormAuthenticationTicketVersion.Member)
                        {
                            var userData = JsonConvert.DeserializeObject<MemberLoginInformation>(authTicket.UserData);
                            HttpContext.Current.User = new UserPrincipal<MemberLoginInformation>(new FormsIdentity(authTicket), userData);
                        }
                        //else
                        //{
                        //    var userData = JsonConvert.DeserializeObject<ContractorLoginInformation>(authTicket.UserData);
                        //    HttpContext.Current.User = new UserPrincipal<ContractorLoginInformation>(new FormsIdentity(authTicket), userData);
                        //}

                        return true;
                    }
                }
                catch (CryptographicException cex)
                {
                    FormsAuthentication.SignOut();
                }
            }

            httpContext.Items["returnUrl"] = httpContext.Request.Url.PathAndQuery;
            return false;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!string.IsNullOrEmpty(Roles))
            {
                string[] listRole = Array.ConvertAll(Roles.Split(','), p => p.Trim());
                foreach (var role in listRole)
                {
                    if (HttpContext.Current.User.IsInRole(role))
                    {
                        return;
                    }
                }

                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary(new
            {
                controller = "Access",
                action = "SignIn",
                returnUrl = filterContext.HttpContext.Items["returnUrl"]
            });
            filterContext.Result = new RedirectToRouteResult(routeValues);
            //base.HandleUnauthorizedRequest(filterContext);
        }
    }
}