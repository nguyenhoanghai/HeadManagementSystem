using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeadManagementSystem.Constant.Authentication
{
    public class BaseLoginInformation
    {
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public List<string> Roles { get; set; }
        public int? MemberId { get; set; }
        public int? ContractorId { get; set; }
    }
}