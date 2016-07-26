using ONEbook.UM.Services;
using ONEbook.UM.Services.Impl;
using ParlayRMS.DTO;
using ParlayRMS.Models;
using ParlayRMS.Models.Users;
using ParlayRMS.Services.Implement.Users;
using ParlayRMS.Services.Interface.Users;
using ParlayRMS.ViewModel.Account;
using ParlayRMS.Web.Authorization;
using ParlayRMS.Web.Context;
using ParlayRMS.Web.Session;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ParlayRMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMaintenanceService _maintenanceService = new MaintenanceService();
        private readonly IAuthorizationService _authorizationService = new AuthorizationService();
        private readonly ICurrentContext _currentContext = new CurrentContext();
        private readonly ICurrentSession _currentSession = new CurrentSession();

        [HttpGet]
        public ActionResult Login()
        {
            return PartialView();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = 0;
                var result = Result.Empty;
                var request = new Request
                {
                    Browser = Request.Browser.Browser,
                    CountryCode = _currentContext.CountryCode,
                    Domain = _currentContext.FullDomainName,
                    Ip = _currentContext.Ip4Address
                };

                var user = _authorizationService.ValidateUser(model.UserName, model.Password, _currentSession.NetSessionId, request, out result, out userId);
                if (result.IsSuccessful)
                {
                    CreateSession(user, userId);
                    CreateAuthInfo(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message;
                    return PartialView(model);
                }
            }

            return PartialView(model);
        }

        public ActionResult Offline()
        {
            if (_maintenanceService.IsUnderMaintenance())
            {
                var underMaintenanceTime = _maintenanceService.GetUnderMaintenanceTime();
                ViewBag.UmFromTime = underMaintenanceTime.From;
                ViewBag.UmToTime = underMaintenanceTime.To;
                return PartialView();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public JsonResult ReCheckInOffline()
        {
            return Json(new
            {
                code = 0,
                message = string.Empty
            });
        }

        public JsonResult ReCheckIn()
        {
            var userId = _currentSession.OnUserId;
            var userName = _currentSession.UserName;

            var re_CheckIn = Utility.AppSetting.IsReCheckIn;
            if (!re_CheckIn)
            {
                return Json(new
                {
                    code = StatusKickedOut.Continue
                });
            }

            if (userId <= 0)
            {
                ClearSessionAndCookies();
                return Json(new
                {
                    code = StatusKickedOut.KickedOut
                });
            }

            if (_authorizationService.ReLogin(userId) == false)
            {
                return Json(new
                {
                    code = StatusKickedOut.Continue
                });
            }
            else
            {
                LogOut(userId, userName);
                return Json(new
                {
                    code = StatusKickedOut.KickedOut
                });
            }
        }

        public ActionResult SignOut()
        {
            var userId = _currentSession.OnUserId;
            var userName = _currentSession.UserName;
            if (userId > 0)
            {
                LogOut(userId, userName);
            }
            else
            {
                ClearSessionAndCookies();
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        private void LogOut(int userId, string userName)
        {
            try
            {
                _authorizationService.Logout(userId, userName);
            }
            catch
            {
                throw;
            }
            finally
            {
                ClearSessionAndCookies();
            }
        }

        private void ClearSessionAndCookies()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
        }

        private void CreateAuthInfo(User user)
        {
            var authInfo = new AuthInfo { UserId = user.Id, GroupId = 1, GroupName = "group name" };

            var authTicket = new FormsAuthenticationTicket(1, user.Name, DateTime.Now, DateTime.Now.AddMinutes(20000), false, authInfo.ToXml());
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            Response.Cookies.Add(authCookie);
        }

        private void CreateSession(User user, int userId)
        {
            _currentSession.UserName = user.Name;
            _currentSession.UserId = user.Id;
            _currentSession.CustId = user.Id;
            _currentSession.OnUserId = userId;
        }
    }
}