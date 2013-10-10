// --------------------------------------------------------------------------
// <copyright file="IUsersDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.Users
{
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Users;

    public interface IUsersDataAccess
    {
        /// <summary>
        /// Gets user by username and group ID.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>User associated with passed username and group ID.</returns>
        IEnumerable<RMS_UserInfoSel> RMS_UserInfoSel(string userName, int groupId);

        /// <summary>
        /// Updates user password.
        /// </summary>
        /// <param name="adminId">The ID of administrator.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="password">New password.</param>
        /// <param name="ip">IP of machine which updates the password.</param>
        /// <returns>Whether or not the password is updated successfully.</returns>
        Result RMS_UserInfoUpdUserPwd(int adminId, int userId, string password, string ip);

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="adminId">The ID of administrator who creates the user.</param>
        /// <param name="userName">Username value.</param>
        /// <param name="password">Default password.</param>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <param name="ip">IP of machine which creates user.</param>
        /// <returns>Whether or not the user is created successfully.</returns>
        Result RMS_UserInfoAddNew(int adminId, string userName, string password, string firstName, string lastName, int groupId, string ip);

        /// <summary>
        /// Updates user information.
        /// </summary>
        /// <param name="adminId">The ID of administrator who updates the user.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="ip">IP of machine which updates user.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not the user is updated successfully.</returns>
        Result RMS_UserInfoUpdAll(int adminId, int userId, string firstName, string lastName, string ip, int groupId);

        /// <summary>
        /// Updates user status.
        /// </summary>
        /// <param name="adminId">The ID of administrator who creates the user's status.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="active">Whether the user is activated or not.</param>
        /// <param name="ip">IP of machine which updates user's status.</param>
        /// <returns>Whether or not the user's status is updated successfully.</returns>
        Result RMS_UserInfoUpdStatus(int adminId, int userId, bool active, string ip);

        /// <summary>
        /// Adds an existing user to the system.
        /// </summary>
        /// <param name="adminId">The ID of administrator who adds the user.</param>
        /// <param name="userName">Username value.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <param name="ip">IP of machine which adds user.</param>
        /// <returns>Whether or not the user is added successfully.</returns>
        Result RMS_UserInfoAddExisting(int adminId, string userName, int groupId, string ip);

        /// <summary>
        /// Gets user's policy.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>Policy of user.</returns>
        IEnumerable<RMS_UserPolicySel> RMS_UserPolicySel(int userId);

        /// <summary>
        /// Updates user's policy.
        /// </summary>
        /// <param name="adminId">The ID of administrator who updates the user's policy.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="policy">New policy.</param>
        /// <param name="ip">IP of machine which updates user's policy.</param>
        /// <returns>Whether or not the user's policy is updated successfully.</returns>
        Result RMS_UserPolicyUpd(int adminId, int userId, string policy, string ip);

        /// <summary>
        /// Gets available menus of a user.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>List of menus and functions that are available to the user.</returns>
        RMS_MenuSelByUserID RMS_MenuSelByUserID(int userId);

        /// <summary>
        /// Gets user information by user ID.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>User information.</returns>
        RMS_UserInfoSelByUserID RMS_UserInfoSelByUserID(int userId);

        /// <summary>
        /// Validates user login information.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <param name="password">Password value.</param>
        /// <param name="loginIp">The IP of machine which the user using to log in.</param>
        /// <param name="loginDomain">Login domain.</param>
        /// <param name="browserName">The name of browser which the user using to log in.</param>
        /// <param name="platform">Platform of machine which the user using to log in.</param>
        /// <param name="countryCode">Country code of place where the machine is staying.</param>
        /// <param name="language">Language ID.</param>
        /// <param name="result">Whether or not the passed information indicates a valid user.</param>
        /// <returns>User Information.</returns>
        RMS_UserInfoSelByUserID RMS_UserLogin(
                                    string userName,
                                    string password,
                                    string loginIp,
                                    string loginDomain,
                                    string browserName,
                                    string platform,
                                    string countryCode,
                                    string language,
                                    out Result result);

        /// <summary>
        /// ReLogin process.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>Whether or re-login successfully.</returns>
        bool RMS_UserRecheckIn(int userId);

        /// <summary>
        /// Check in process.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="netSessionId">Session ID value.</param>
        /// <param name="ip">The IP of machine which the user using to log out.</param>
        /// <returns>Whether or not the user logged out successful.</returns>
        int RMS_UserCheckIn(int userId, string netSessionId, string ip);

        /// <summary>
        /// Logout process.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="userName">Username value.</param>
        void RMS_UserLogout(int userId, string userName);

        /// <summary>
        /// Gets user information by username.
        /// </summary>
        /// <param name="username">Username value.</param>
        /// <returns>User information.</returns>
        RMS_CustomerSelInfoByUsername RMS_CustomerSelInfoByUsername(string username);
    }
}