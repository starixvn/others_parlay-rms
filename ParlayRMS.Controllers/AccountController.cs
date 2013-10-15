// --------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using ParlayRMS.DTO;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Users;
    using ParlayRMS.Services.Implement.Systems;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.ViewModel.Account;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Web.Context;
    using ParlayRMS.Web.Session;

    /// <summary>
    /// Account Controller.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IAuthorizationService _authorizationService = new AuthorizationService();
        private readonly MaintenanceService _maintenanceService = new MaintenanceService();
        private readonly ICurrentContext _currentContext = new CurrentContext();
        private readonly ICurrentSession _currentSession = new CurrentSession();

        #region Login

        /// <summary>
        /// GET method: gets login form.
        /// </summary>
        /// <returns>Login page.</returns>
        [HttpGet]
        public ActionResult Login()
        {
            var maintenanceInfo = _maintenanceService.GetMaintenanceInfo();
            if (maintenanceInfo.IsDownTime)
            {
                return this.RedirectToAction("Offline", "Account");
            }

            return this.PartialView();
        }

        /// <summary>
        /// POST method: submits the login form.
        /// </summary>
        /// <param name="model">Model that contains the login information.</param>
        /// <returns>Whether the user login successfully or not.</returns>
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
                    CountryCode = this._currentContext.CountryCode,
                    Domain = this._currentContext.FullDomainName,
                    Ip = this._currentContext.Ip4Address
                };

                var user = this._authorizationService.ValidateUser(model.UserName, model.Password, this._currentSession.NetSessionId, request, out result, out userId);
                if (result.IsSuccessful)
                {
                    this.CreateSession(user, userId);
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

        #region UM Maintenance

        /// <summary>
        /// Redirects to Under Maintenance.
        /// </summary>
        /// <returns>Under Maintenance view.</returns>
        public ActionResult Offline()
        {
            var maintenanceInfo = _maintenanceService.GetMaintenanceInfo();
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
        /// Checks whether the system is in the offline time.
        /// </summary>
        /// <returns>Status index.</returns>
        public int ReCheckInOffline()
        {
            var maintenanceInfo = _maintenanceService.GetMaintenanceInfo();
            if (!maintenanceInfo.IsDownTime)
            {
                return (int)StatusKickedOut.Login;
            }

            return (int)StatusKickedOut.Maintenance;
        }

        #endregion UM Maintenance

        #region Logout

        /// <summary>
        /// Re-check the login status.
        /// </summary>
        /// <returns>Status index.</returns>
        public int ReCheckIn()
        {
            var maintenanceInfo = _maintenanceService.GetMaintenanceInfo();
            var userId = this._currentSession.OnUserId;
            var userName = this._currentSession.UserName;

            if (maintenanceInfo.IsDownTime)
            {
                this.ClearSessionAndCookies();

                this.LogOut(userId, userName);
                return (int)StatusKickedOut.Maintenance;
            }

            var re_CheckIn = Utility.AppSetting.IsReCheckIn;
            if (!re_CheckIn)
            {
                return (int)StatusKickedOut.Continue;
            }

            if (userId <= 0)
            {
                this.ClearSessionAndCookies();
                return (int)StatusKickedOut.KickedOut;
            }

            if (this._authorizationService.ReLogin(userId) == false)
            {
                return (int)StatusKickedOut.Continue;
            }
            else
            {
                this.LogOut(userId, userName);
                return (int)StatusKickedOut.KickedOut;
            }
        }

        /// <summary>
        /// Signs the user out.
        /// </summary>
        /// <returns>Redirect to Login form.</returns>
        public ActionResult SignOut()
        {
            var userId = this._currentSession.OnUserId;
            var userName = this._currentSession.UserName;
            if (userId > 0)
            {
                this.LogOut(userId, userName);
            }
            else
            {
                this.ClearSessionAndCookies();
            }

            return this.Redirect(FormsAuthentication.LoginUrl);
        }

        /// <summary>
        /// Signs the user out.
        /// </summary>
        /// <param name="userId">The on user identifier.</param>
        /// <param name="userName">The username.</param>
        private void LogOut(int userId, string userName)
        {
            try
            {
                this._authorizationService.Logout(userId, userName);
            }
            catch (Exception ex)
            {
                throw; ////ex;
            }
            finally
            {
                this.ClearSessionAndCookies();
            }
        }

        /// <summary>
        /// Clears the session and cookies.
        /// </summary>
        private void ClearSessionAndCookies()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
        }

        /// <summary>
        /// Creates the authentication information.
        /// </summary>
        /// <param name="user">The current login user.</param>
        private void CreateAuthInfo(User user)
        {
            var authInfo = new AuthInfo { UserId = user.Id, GroupId = 1, GroupName = "group name" };

            var authTicket = new FormsAuthenticationTicket(1, user.Name, DateTime.Now, DateTime.Now.AddMinutes(20000), false, authInfo.ToXml());
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            Response.Cookies.Add(authCookie);
        }

        /// <summary>
        /// Creates new session.
        /// </summary>
        /// <param name="user">The login user.</param>
        /// <param name="userId">The on user identifier.</param>
        private void CreateSession(User user, int userId)
        {
            this._currentSession.UserName = user.Name;
            this._currentSession.UserId = user.Id;
            this._currentSession.CustId = user.Id;
            this._currentSession.OnUserId = userId;
        }

        #endregion Logout
    }
}