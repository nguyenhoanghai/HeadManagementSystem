using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HeadManagementSystem.Constant.Authentication
{
    public class FormAuthenticationService  
    {
        #region constructor  
        static object key = new object();
        private static volatile FormAuthenticationService _Instance;  //volatile =>  tranh dung thread
        public static FormAuthenticationService Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new FormAuthenticationService();

                return _Instance;
            }
        }
        private FormAuthenticationService() { }
        #endregion
        public void AddAuthentication<T>(HttpResponseBase response, bool rememberMe, T userData) where T : BaseLoginInformation
        {
            string userDataJSON = JsonConvert.SerializeObject(userData);
            FormsAuthentication.SetAuthCookie(userData.LoginName, rememberMe);
            FormAuthenticationTicketVersion formAuthenticationTicketVersion;
            if (userData is MemberLoginInformation)
            {
                formAuthenticationTicketVersion = FormAuthenticationTicketVersion.Member;
            }
            //else if (userData is ContractorLoginInformation)
            //{
            //    formAuthenticationTicketVersion = FormAuthenticationTicketVersion.Contractor;
            //}
            else
            {
                formAuthenticationTicketVersion = FormAuthenticationTicketVersion.Admin;
            }
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                (int)formAuthenticationTicketVersion,
                userData.LoginName,
                DateTime.Now,
                (rememberMe ? DateTime.Now.AddDays(AppSetting.DaysExpiredRememberMeLogin) : DateTime.Now.AddMinutes(20)),
                rememberMe,
                userDataJSON,
                FormsAuthentication.FormsCookiePath);
            string encTicket = FormsAuthentication.Encrypt(ticket);

            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            if (ticket.IsPersistent && rememberMe)
            {
                authCookie.Expires = ticket.Expiration;
            }
            response.Cookies.Add(authCookie);
        }

        public void LogOffAuthen(HttpSessionStateBase session, HttpResponseBase response)
        {
            session.Abandon();
            FormsAuthentication.SignOut();
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            response.Cookies.Add(cookie);
        }

        public void UpdateAuthentication<T>(T userData) where T : BaseLoginInformation
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var newticket = new FormsAuthenticationTicket(
                        authTicket.Version,
                        authTicket.Name,
                        authTicket.IssueDate,
                        authTicket.Expiration,
                        authTicket.IsPersistent,
                        JsonConvert.SerializeObject(userData),
                        authTicket.CookiePath);
                    authCookie.Value = FormsAuthentication.Encrypt(newticket);
                    authCookie.Expires = newticket.Expiration;
                    HttpContext.Current.Response.Cookies.Set(authCookie);
                }
            }
        }

    }
}