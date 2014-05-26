// --------------------------------------------------------------------------
// <copyright file="IUsersService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.Users
{
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Users;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.Users;

    public interface IUsersService
    {
        /// <summary>
        /// Gets user by ID.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>User object of passed ID.</returns>
        User GetUserById(int userId);

        /// <summary>
        /// Gets user by username and group ID.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>User associated with passed username and group ID.</returns>
        IEnumerable<User> GetListUser(string userName, int groupId);

        /// <summary>
        /// Resets user password.
        /// </summary>
        /// <param name="adminId">The ID of administrator.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="password">New password.</param>
        /// <param name="ip">IP of machine which update the password.</param>
        /// <returns>Whether or not the password is updated successfully.</returns>
        Result ResetPassword(int adminId, int userId, string password, string ip);

        /// <summary>
        /// Updates user status.
        /// </summary>
        /// <param name="adminId">The ID of administrator who creates the user's status.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="active">Whether the user is activated or not.</param>
        /// <param name="ip">IP of machine which updates user's status.</param>
        /// <returns>Whether or not the user's status is updated successfully.</returns>
        Result UpdateStatus(int adminId, int userId, bool active, string ip);

        /// <summary>
        /// Updates user information.
        /// </summary>
        /// <param name="adminId">The ID of administrator who updates the user.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <param name="ip">IP of machine which updates user.</param>
        /// <returns>Whether or not the user is updated successfully.</returns>
        Result EditUser(int adminId, int userId, string firstName, string lastName, int groupId, string ip);

        /// <summary>
        /// Adds new user.
        /// </summary>
        /// <param name="adminId">The ID of administrator who creates the user.</param>
        /// <param name="userName">Username value.</param>
        /// <param name="password">Default password.</param>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <param name="ip">IP of machine which creates user.</param>
        /// <returns>Whether or not the user is created successfully.</returns>
        Result CreateUser(int adminId, string userName, string password, string firstName, string lastName, int groupId, string ip);

        /// <summary>
        /// Adds an existing user to the system.
        /// </summary>
        /// <param name="adminId">The ID of administrator who adds the user.</param>
        /// <param name="userName">Username value.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <param name="ip">IP of machine which adds user.</param>
        /// <returns>Whether or not the user is added successfully.</returns>
        Result CreateExistUser(int adminId, string userName, int groupId, string ip);

        /// <summary>
        /// Gets user's functions.
        /// </summary>
        /// <param name="userId">Passed user ID.</param>
        /// <returns>List of functions that available to the user.</returns>
        IEnumerable<Function> GetUserFunction(int userId);

        /// <summary>
        /// Updates user's policy.
        /// </summary>
        /// <param name="adminId">The ID of administrator who updates the user's policy.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="policy">New policy.</param>
        /// <param name="ip">IP of machine which updates user's policy.</param>
        /// <returns>Whether or not the user's policy is updated successfully.</returns>
        Result UpdateUserPolicy(int adminId, int userId, string policy, string ip);

        /// <summary>
        /// Gets user cache.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>User Cache.</returns>
        UserCache GetUserCache(int userId);

        /// <summary>
        /// Checks if the user has the permission on a specific function or not.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="functionKey">Function key value.</param>
        /// <returns>The user has permission on a specific function or not.</returns>
        bool HasWritePermission(int userId, string functionKey);

        /// <summary>
        /// Checks if the user has the permission on a specific function or not.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="functionKey">Function key value.</param>
        /// <returns>The user has permission on a specific function or not.</returns>
        bool HasReadPermission(int userId, string functionKey);

        /// <summary>
        /// Gets customer information.
        /// </summary>
        /// <param name="username">Username value.</param>
        /// <returns>Customer Information.</returns>
        RMS_CustomerSelInfoByUsername GetCustomerInfo(string username);
    }
}