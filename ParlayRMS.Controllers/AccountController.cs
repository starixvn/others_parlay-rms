namespace ParlayRMS.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using ParlayRMS.DTO;
    using ParlayRMS.Models;
    using ParlayRMS.Services.Implement;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.ViewModel.Account;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Web.Context;
    using ParlayRMS.Web.Session;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Implement.Systems;
    using ParlayRMS.Models.Users;

    public class AccountController : Controller
    {
        private readonly IAuthorizationService authorizationService = new AuthorizationService();
        private readonly MaintenanceService maintenanceService = new MaintenanceService();
        private readonly ICurrentSession currentSession = CurrentSession.Instance;
        private readonly ICurrentContext currentContext = CurrentContext.Instance;

        #region Login

        // GET: /Account/Login
        [HttpGet]
        public ActionResult Login()
        {
            var maintenanceInfo = maintenanceService.GetMaintenanceInfo();
            if (maintenanceInfo.IsDownTime)
            {
                return this.RedirectToAction("Offline", "Account");
            }

            return this.PartialView();
        }

        // POST: /Account/Login
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var onUserId = 0;
                var result = Result.Empty;
                var request = new Request
                {
                    Browser = Request.Browser.Browser,
                    CountryCode = this.currentContext.CountryCode,
                    Domain = this.currentContext.FullDomainName,
                    Ip = this.currentContext.Ip4Address
                };

                var user = this.authorizationService.ValidateUser(model.UserName, model.Password, this.currentSession.NetSessionId, request, out result, out onUserId);
                if (result.IsSuccessful)
                {
                    this.CreateSession(user, onUserId);
                    this.CreateAuthInfo(user);
                    return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message;
                    return this.PartialView(model);
                }
            }

            return this.PartialView(model);
        }

        #endregion Login

        #region Logout

        public int ReCheckIn()
        {
            var maintenanceInfo = maintenanceService.GetMaintenanceInfo();
            var onUserId = this.currentSession.OnUserId;
            var userName = this.currentSession.UserName;

            if (maintenanceInfo.IsDownTime)
            {
                this.ClearSessionAndCookies();

                this.LogOut(onUserId, userName);
                return (int)StatusKickedOut.Maintenance;
            }

            var re_CheckIn = Utility.AppSetting.IsReCheckIn;
            if (!re_CheckIn)
            {
                return (int)StatusKickedOut.Continue;
            }

            if (onUserId <= 0)
            {
                this.ClearSessionAndCookies();
                return (int)StatusKickedOut.KickedOut;
            }

            if (this.authorizationService.ReLogin(onUserId) == false)
            {
                return (int)StatusKickedOut.Continue;
            }
            else
            {
                this.LogOut(onUserId, userName);
                return (int)StatusKickedOut.KickedOut;
            }
        }

        public ActionResult SignOut()
        {
            var onUserId = this.currentSession.OnUserId;
            var userName = this.currentSession.UserName;
            if (onUserId > 0)
            {
                this.LogOut(onUserId, userName);
            }
            else
            {
                this.ClearSessionAndCookies();
            }

            return this.Redirect(FormsAuthentication.LoginUrl);
        }

        private void LogOut(int onUserId, string userName)
        {
            try
            {
                this.authorizationService.Logout(onUserId, userName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.ClearSessionAndCookies();
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

        private void CreateSession(User user, int onUserId)
        {
            this.currentSession.UserName = user.Name;
            this.currentSession.UserId = user.Id;
            this.currentSession.CustId = user.Id;
            this.currentSession.OnUserId = onUserId;
        }

        #endregion Logout

        #region UM Maintenance

        /// <summary>
        /// Under Maintenance
        /// </summary>
        /// <returns></returns>
        public ActionResult Offline()
        {
            var maintenanceInfo = maintenanceService.GetMaintenanceInfo();
            if (maintenanceInfo.IsDownTime)
            {
                ViewBag.UmFromTime = maintenanceInfo.TimeFrom;
                ViewBag.UmToTime = maintenanceInfo.TimeTo;
                return this.PartialView();
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int ReCheckInOffline()
        {
            var maintenanceInfo = maintenanceService.GetMaintenanceInfo();
            if (!maintenanceInfo.IsDownTime)
            {
                return (int)StatusKickedOut.Login;
            }

            return (int)StatusKickedOut.Maintenance;
        }

        #endregion UM Maintenance
    }
}