using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeadManagementSystem.Constant.Authentication
{
    public static class AuthenticationHelper<T> where T : BaseLoginInformation
    {
        public static T UserData
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return ((UserPrincipal<T>)(HttpContext.Current.User)).UserData;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}