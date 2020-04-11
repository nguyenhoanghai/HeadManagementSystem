using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace HeadManagementSystem.Constant.Authentication
{
    public class UserPrincipal<T> : IPrincipal where T : BaseLoginInformation
    {
        public UserPrincipal(IIdentity identity, T userData)
        {
            Identity = identity;
            UserData = userData;
        }

        public IIdentity Identity
        {
            get;
            private set;
        }

        public T UserData { get; set; }

        public bool IsInRole(string role)
        {
            return UserData.Roles.Contains(role);
        }
    }
}