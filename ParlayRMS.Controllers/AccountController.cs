using Fanex.Common;
using Newtonsoft.Json;
using ONEbook.UM.Services.Impl;
using ParlayRMS.DTO;
using ParlayRMS.DTO.Users;
using ParlayRMS.Models;
using ParlayRMS.Models.Users;
using ParlayRMS.Services.Implement.Users;
using ParlayRMS.ViewModel.Account;
using ParlayRMS.Web.Authorization;
using ParlayRMS.Web.Context;
using ParlayRMS.Web.Session;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ParlayRMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly MaintenanceService _maintenanceService = new MaintenanceService();
        private readonly AuthorizationService _authorizationService = new AuthorizationService();
        private readonly CurrentContext _currentContext = new CurrentContext();
        private readonly CurrentSession _currentSession = new CurrentSession();

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

                    var aspNetCoreAuth = GetAspNetCoreAuth();
                    if (!string.IsNullOrEmpty(aspNetCoreAuth))
                    {
                        string aspNetCoreAuthUrl = GetAspNetCoreAuthUrl(model, request, aspNetCoreAuth);
                        return Redirect(aspNetCoreAuthUrl);
                    }
                    else
                    {
                        return Redirect(FormsAuthentication.DefaultUrl);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message;
                    return PartialView(model);
                }
            }

            return PartialView(model);
        }

        private string GetAspNetCoreAuthUrl(LoginModel model, Request request, string aspNetCoreAuth)
        {
            var redirectUrl = GetNetCoreAuthRedirectUrl(aspNetCoreAuth, FormsAuthentication.DefaultUrl);

            var command = new UserLoginParameter
            {
                Username = model.UserName,
                Userpwd = Cryptography.CfsEnCode(model.Password),
                LoginIp = request.Ip,
                LoginDomain = request.Domain,
                BrowserName = request.Browser,
                Platform = request.Platform,
                CountryCode = request.CountryCode,
                Language = request.Language
            };

            var jsonCommand = JsonConvert.SerializeObject(command);
            var loginParameters = HttpUtility.UrlEncode(jsonCommand);

            var aspNetCoreAuthUrl = aspNetCoreAuth
                + $"/Login?loginParameters={loginParameters}&redirectUrl={redirectUrl}";
            return aspNetCoreAuthUrl;
        }

        private string GetAspNetCoreAuth()
        {
            var aspNetCoreAuth = ConfigurationManager.AppSettings["AspNetCoreAuth"];
            return !string.IsNullOrEmpty(aspNetCoreAuth) ? $"{aspNetCoreAuth}/PropagationAuth" : string.Empty;
        }

        private string GetNetCoreAuthRedirectUrl(string aspNetCoreAuth, string defaultUrl)
        {
            var redirectUrl = new Uri(Request.Url, defaultUrl).ToString();
            var isSsl = aspNetCoreAuth.StartsWith("https", StringComparison.CurrentCultureIgnoreCase)
                && redirectUrl.IndexOf("https", StringComparison.CurrentCultureIgnoreCase) < 0;
            if (isSsl)
            {
                redirectUrl = Regex.Replace(redirectUrl, "http", "https", RegexOptions.IgnoreCase);
            }

            return HttpUtility.UrlEncode(redirectUrl);
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

            var aspNetCoreAuth = GetAspNetCoreAuth();
            if (!string.IsNullOrEmpty(aspNetCoreAuth))
            {
                var redirectUrl = GetNetCoreAuthRedirectUrl(aspNetCoreAuth, FormsAuthentication.LoginUrl); ;

                return Redirect(aspNetCoreAuth + $"/LogOut?redirectUrl={redirectUrl}");
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