// --------------------------------------------------------------------------
// <copyright file="AuthorizationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.Users
{
    using ParlayRMS.DataAccess.Implement.Users;
    using ParlayRMS.DataAccess.Interface.Users;
    using ParlayRMS.DTO;
    using ParlayRMS.Models.Groups;
    using ParlayRMS.Models.Users;
    using ParlayRMS.Services.Interface.Users;

    /// <summary>
    /// Authorization Service.
    /// </summary>
    public class AuthorizationService : BaseService, IAuthorizationService
    {
        private readonly IUsersDataAccess _usersDataAccess = new UsersDataAccess();

        /// <summary>
        /// Login user process.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="sessionId">Session ID value.</param>
        /// <param name="ip">The IP of machine which the user using to log out.</param>
        /// <returns>Whether or not the user logged out successful.</returns>
        public int LoginOnUser(int userId, string sessionId, string ip)
        {
            return this._usersDataAccess.RMS_UserCheckIn(userId, sessionId, ip);
        }

        /// <summary>
        /// Logout process.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="userName">Username value.</param>
        public void Logout(int userId, string userName)
        {
            if (userId > 0)
            {
                this._usersDataAccess.RMS_UserLogout(userId, userName);
            }
        }

        /// <summary>
        /// ReLogin process.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>Whether or re-login successfully.</returns>
        public bool ReLogin(int userId)
        {
            return this._usersDataAccess.RMS_UserRecheckIn(userId);
        }

        /// <summary>
        /// Validates user information.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <param name="passWord">Password value.</param>
        /// <param name="netSessionId">Net session ID.</param>
        /// <param name="request">Request which contains information about user's machine.</param>
        /// <param name="result">Whether or not the passed information indicates a valid user.</param>
        /// <param name="userId">The onUser ID.</param>
        /// <returns>User Information.</returns>
        public User ValidateUser(string userName, string passWord, string netSessionId, Request request, out Result result, out int userId)
        {
            userId = 0;
            var data = this._usersDataAccess.RMS_UserLogin(
                                                 userName, 
                                                 passWord, 
                                                 request.Ip, 
                                                 request.Domain, 
                                                 request.Browser, 
                                                 request.Platform, 
                                                 request.CountryCode, 
                                                 request.Language, 
                                                 out result);

            if (result.IsSuccessful)
            {
                userId = this.LoginOnUser(data.CustId, netSessionId, request.Ip);
                return new User
                {
                    Id = data.CustId,
                    Name = data.UserName,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Group = new Group
                    {
                        Id = data.GroupId,
                        Name = data.GroupName
                    },
                    IsActive = data.Enabled
                };
            }

            return null;
        }
    }
}