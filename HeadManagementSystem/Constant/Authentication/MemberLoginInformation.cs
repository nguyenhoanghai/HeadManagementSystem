using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeadManagementSystem.Constant.Authentication
{
    public class MemberLoginInformation : BaseLoginInformation
    {
        public bool IsHaveMemberMedicalRelease { get; set; }
        public bool IsHaveBeckPolicyRelease { get; set; }
    }
}