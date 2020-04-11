using GPRO.Ultilities;
using HeadManagementSystem.Constant.Authentication;
using HeadManagementSystem.Models;
using HMS.Data.BLL;
using HMS.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace HeadManagementSystem.Controllers
{
    public class AccessController : BaseController
    {

        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SignIn(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userLoginInfo = BLLAccount.Instance.FindAccount(App_Global.AppGlobal.Connectionstring, model.LoginName, model.Password);
                    if (userLoginInfo != null)
                    {
                        string avatar = userLoginInfo.Data1 as string;
                        var login = new BaseLoginInformation() { UserId = userLoginInfo.Id, LoginName = userLoginInfo.Code, FullName = userLoginInfo.Name, Roles = new List<string>(), Avatar = (!string.IsNullOrEmpty(avatar) ? avatar : "") };
                        switch (userLoginInfo.Data)
                        {
                            case (int)UserRole.Admin: login.Roles.Add(UserRoleConstant.Admin); break;
                            case (int)UserRole.Member: login.Roles.Add(UserRoleConstant.Member); break;
                        }
                        AddAuthentication(login, model.RememberMe);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                        return View(model);
                }
                catch (Exception ex)
                {
                    // dc50Exception.AddToModelState(ModelState);
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        private void AddAuthentication(BaseLoginInformation baseLoginInformation, bool rememberMe)
        {
            if (baseLoginInformation.Roles.Contains(UserRoleConstant.Contractor))
            {
                //var contractorLoginInfor = _userService.GetContractorLogin(baseLoginInformation);
                //contractorLoginInfor.UserId = baseLoginInformation.UserId;
                //_iDC50AuthenticationService.AddAuthentication<ContractorLoginInformation>(Response, rememberMe, contractorLoginInfor);
            }
            else if (baseLoginInformation.Roles.Contains(UserRoleConstant.Member))
            {
                //  var memberLoginInfor = _userService.GetMemberLogin(baseLoginInformation);
                // memberLoginInfor.UserId = baseLoginInformation.UserId;
                FormAuthenticationService.Instance.AddAuthentication<MemberLoginInformation>(Response, rememberMe, new MemberLoginInformation() { UserId = baseLoginInformation.UserId, LoginName = baseLoginInformation.LoginName, FullName = baseLoginInformation.FullName, Roles = baseLoginInformation.Roles, Avatar = baseLoginInformation.Avatar });
            }
            else
            {
                FormAuthenticationService.Instance.AddAuthentication<BaseLoginInformation>(Response, rememberMe, baseLoginInformation);
            }
        }

        public ActionResult SignOut()
        {
            FormAuthenticationService.Instance.LogOffAuthen(Session, Response);
            return RedirectToAction("SignIn", "Access", null);
        }

        public ActionResult Profile()
        {
            int userId = 0;
            dynamic member = null;
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole(UserRoleConstant.SuperAdmin))
                {

                }
                else if (User.IsInRole(UserRoleConstant.Admin))
                {
                    member = AuthenticationHelper<BaseLoginInformation>.UserData;
                    userId = member.UserId;
                }
                else if (User.IsInRole(UserRoleConstant.Contractor))
                {

                }
                else
                {
                    member = AuthenticationHelper<MemberLoginInformation>.UserData;
                    userId = member.UserId;
                }
            }
            if (userId > 0)
                return View(BLLAccount.Instance.FindAccount(App_Global.AppGlobal.Connectionstring, userId));
            return View(new AccountModel());
        }

        [HttpPost]
        public JsonResult UploadFiles()
        {
            int code = 0;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var fileUp = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];
                try
                {
                    if (fileUp != null)
                    {
                        if (!System.IO.Directory.Exists(Server.MapPath("~/UploadedFiles")))
                            System.IO.Directory.CreateDirectory(Server.MapPath("~/UploadedFiles"));
                        string ext = fileUp.FileName.Substring(fileUp.FileName.LastIndexOf('.') + 1);
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(fileName));
                        fileUp.SaveAs(path);

                        dynamic member = null;
                        if (Request.IsAuthenticated)
                        {
                            if (User.IsInRole(UserRoleConstant.SuperAdmin))
                            {

                            }
                            else if (User.IsInRole(UserRoleConstant.Admin))
                            {
                                member = AuthenticationHelper<BaseLoginInformation>.UserData;
                            }
                            else if (User.IsInRole(UserRoleConstant.Contractor))
                            {

                            }
                            else
                            {
                                member = AuthenticationHelper<MemberLoginInformation>.UserData;
                            }
                        }
                        if (BLLAccount.Instance.UpAvatar(App_Global.AppGlobal.Connectionstring, member.UserId, fileName))
                        {
                            code = 1;
                            UpdateAuthenticationInfo(fileName);
                        }
                        else
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return Json(code);
        }

        private void UpdateAuthenticationInfo(string fileName)
        {
            dynamic member = null;
            if (User.IsInRole(UserRoleConstant.SuperAdmin))
            {

            }
            else if (User.IsInRole(UserRoleConstant.Admin))
            {
                member = AuthenticationHelper<BaseLoginInformation>.UserData;
                member.Avatar = fileName;
                FormAuthenticationService.Instance.UpdateAuthentication<BaseLoginInformation>(member);
            }
            else if (User.IsInRole(UserRoleConstant.Contractor))
            {

            }
            else
            {
                member = AuthenticationHelper<MemberLoginInformation>.UserData;
                member.Avatar = fileName;
                FormAuthenticationService.Instance.UpdateAuthentication<MemberLoginInformation>(member);
            }
        }

        [HttpPost]
        public JsonResult UpdatePassword()
        {
            dynamic member = null;
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole(UserRoleConstant.SuperAdmin))
                {

                }
                else if (User.IsInRole(UserRoleConstant.Admin))
                {
                    member = AuthenticationHelper<BaseLoginInformation>.UserData;
                }
                else if (User.IsInRole(UserRoleConstant.Contractor))
                {

                }
                else
                {
                    member = AuthenticationHelper<MemberLoginInformation>.UserData;
                }
                string oldPass = System.Web.HttpContext.Current.Request.Form["oldPass"].ToString();
                var acc = BLLAccount.Instance.Get(App_Global.AppGlobal.Connectionstring, (int)member.UserId);
                if (acc != null)
                {
                    if (acc.Password == GlobalFunction.EncryptMD5(oldPass))
                    {
                        if (BLLAccount.Instance.UpdatePassword(App_Global.AppGlobal.Connectionstring, (int)member.UserId, System.Web.HttpContext.Current.Request.Form["newPass"].ToString()))
                            return Json(new { isValid = true, url = "/access/signout" });
                        else
                            return Json(new { isValid = false, sms = "Cập nhật mật khẩu thất bại." });
                    }
                    else
                        return Json(new { isValid = false, sms = "Mật khẩu cũ không khớp. Vui lòng thử lại.!" });
                }
                else
                    return Json(new { isValid = false, sms = "Cập nhật mật khẩu thất bại." });
            }
            else
                return Json(new { isValid = true, url = "/access/signout" });
        }

        [HttpPost]
        public JsonResult UpdateInfo()
        {
            dynamic member = null;
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole(UserRoleConstant.SuperAdmin))
                {

                }
                else if (User.IsInRole(UserRoleConstant.Admin))
                {
                    member = AuthenticationHelper<BaseLoginInformation>.UserData;
                }
                else if (User.IsInRole(UserRoleConstant.Contractor))
                {

                }
                else
                {
                    member = AuthenticationHelper<MemberLoginInformation>.UserData;
                }

                var rs = BLLUser.Instance.UpdateInfo(
                    App_Global.AppGlobal.Connectionstring,
                    new NhanVienModel()
                    {
                        Ten = System.Web.HttpContext.Current.Request.Form["name"].ToString(),
                        DienThoai = System.Web.HttpContext.Current.Request.Form["phone"].ToString(),
                        DiaChi = System.Web.HttpContext.Current.Request.Form["address"].ToString(),
                        Note = System.Web.HttpContext.Current.Request.Form["note"].ToString()
                    },
                    (int)member.UserId);
                if (rs.IsSuccess)
                    return Json(new { isValid = true, url = "/access/profile" });
                else
                    return Json(new { isValid = false, sms = rs.sms });
            }
            else
                return Json(new { isValid = true, url = "/access/signout" });
        }

    }
}