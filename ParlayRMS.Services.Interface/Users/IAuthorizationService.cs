// --------------------------------------------------------------------------
// <copyright file="IAuthorizationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.Users
{
    using ParlayRMS.DTO;
    using ParlayRMS.Models.Users;

    public interface IAuthorizationService
    {
        /// <summary>
        /// Login user process.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="sessionId">Session ID value.</param>
        /// <param name="ip">The IP of machine which the user using to log out.</param>
        /// <returns>Whether or not the user logged out successful.</returns>
        int LoginOnUser(int userId, string sessionId, string ip);

        /// <summary>
        /// Logout process.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="userName">Username value.</param>
        void Logout(int userId, string userName);

        /// <summary>
        /// ReLogin process.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>Whether or re-login successfully.</returns>
        bool ReLogin(int userId);

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
        User ValidateUser(string userName, string passWord, string netSessionId, Request request, out Result result, out int userId);
    }
}