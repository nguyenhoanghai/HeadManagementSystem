using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HeadManagementSystem.Constant.Authentication
{
    public enum UserRole
    {
        [Description(UserRoleConstant.Admin)]
        Admin = 100,
        [Description(UserRoleConstant.Member)]
        Member = 1,
        [Description(UserRoleConstant.Contractor)]
        Contractor = 2,
        [Description(UserRoleConstant.ApiUser)]
        ApiUser = 3,
        [Description(UserRoleConstant.SuperAdmin)]
        SuperAdmin = 1000
    }

    public static class UserRoleConstant
    {
        public const string Admin = "Admin";
        public const string Member = "Member";
        public const string Contractor = "Contractor";
        public const string ApiUser = "ApiUser";
        public const string SuperAdmin = "SuperAdmin";
    }
}