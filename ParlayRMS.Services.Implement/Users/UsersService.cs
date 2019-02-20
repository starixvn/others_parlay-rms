// --------------------------------------------------------------------------
// <copyright file="UsersService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using ParlayRMS.DataAccess.Implement.Users;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Users;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.Groups;
    using ParlayRMS.Models.Menus;
    using ParlayRMS.Models.Users;
    using ParlayRMS.Services.Implement.Groups;
    using ParlayRMS.Services.Implement.Systems;
    using ParlayRMS.Utility;
    using ParlayRMS.Web.Session;

    /// <summary>
    /// Users Service.
    /// </summary>
    public class UsersService : BaseService
    {
        private readonly UsersDataAccess _usersDataAccess = new UsersDataAccess();
        private readonly GroupsService _groupsService = new GroupsService();
        private readonly ICurrentSession _currentSession = CurrentSession.Instance;

        /// <summary>
        /// Gets user by ID.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>User object of passed ID.</returns>
        public User GetUserById(int userId)
        {
            var data = this._usersDataAccess.RMS_UserInfoSelByUserID(userId);
            return new User
            {
                Id = userId,
                Name = data.UserName,
                FirstName = data.FirstName,
                LastName = data.LastName,
                IsActive = data.Enabled,
                Group = new Group
                {
                    Id = data.GroupId,
                    Name = data.GroupName
                }
            };
        }

        /// <summary>
        /// Gets user by username and group ID.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>User associated with passed username and group ID.</returns>
        public IEnumerable<User> GetListUser(string userName, int groupId)
        {
            var data = this._usersDataAccess.RMS_UserInfoSel(userName, groupId);
            return data.Select(x => new User
            {
                Id = x.CustId,
                Name = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Group = this._groupsService.GetGroupById(x.GroupId),
                Creator = new User
                {
                    Name = x.Creator
                },
                CreatedDate = x.CreatedDate,
                IsActive = x.Enabled
            });
        }

        /// <summary>
        /// Resets user password.
        /// </summary>
        /// <param name="adminId">The ID of administrator.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="password">New password.</param>
        /// <param name="ip">IP of machine which update the password.</param>
        /// <returns>Whether or not the password is updated successfully.</returns>
        public Result ResetPassword(int adminId, int userId, string password, string ip)
        {
            return this._usersDataAccess.RMS_UserInfoUpdUserPwd(adminId, userId, password, ip);
        }

        /// <summary>
        /// Updates user status.
        /// </summary>
        /// <param name="adminId">The ID of administrator who creates the user's status.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="active">Whether the user is activated or not.</param>
        /// <param name="ip">IP of machine which updates user's status.</param>
        /// <returns>Whether or not the user's status is updated successfully.</returns>
        public Result UpdateStatus(int adminId, int userId, bool active, string ip)
        {
            return this._usersDataAccess.RMS_UserInfoUpdStatus(adminId, userId, active, ip);
        }

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
        public Result EditUser(int adminId, int userId, string firstName, string lastName, int groupId, string ip)
        {
            return this._usersDataAccess.RMS_UserInfoUpdAll(adminId, userId, firstName, lastName, ip, groupId);
        }

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
        public Result CreateUser(int adminId, string userName, string password, string firstName, string lastName, int groupId, string ip)
        {
            return this._usersDataAccess.RMS_UserInfoAddNew(adminId, userName, password, firstName, lastName, groupId, ip);
        }

        /// <summary>
        /// Adds an existing user to the system.
        /// </summary>
        /// <param name="adminId">The ID of administrator who adds the user.</param>
        /// <param name="userName">Username value.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <param name="ip">IP of machine which adds user.</param>
        /// <returns>Whether or not the user is added successfully.</returns>
        public Result CreateExistUser(int adminId, string userName, int groupId, string ip)
        {
            return this._usersDataAccess.RMS_UserInfoAddExisting(adminId, userName, groupId, ip);
        }

        /// <summary>
        /// Gets user's functions.
        /// </summary>
        /// <param name="userId">Passed user ID.</param>
        /// <returns>List of functions that available to the user.</returns>
        public IEnumerable<Function> GetUserFunction(int userId)
        {
            var user = this.GetUserById(_currentSession.UserId);
            var data = this._usersDataAccess.RMS_UserPolicySel(userId);
            if (!AppSetting.HiddenGroups.Contains(user.Group.Name))
            {
                data = data.Where(x => x.FuncKey != FunctionKeys.ManageFunction && x.FuncKey != FunctionKeys.ManageMenu);
            }

            return data.Select(x => new Function
            {
                Id = x.FuncId,
                Name = x.FuncName,
                Parent = new Function
                {
                    Id = x.ParentId,
                    Name = x.ParentName
                },
                UserPolicy = new Policy(x.UserPolicy),
                GroupPolicy = new Policy(x.GroupPolicy),
                DisplayOrder = x.DisplayOrder
            });
        }

        /// <summary>
        /// Checks if the user has the permission on a specific function or not.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="functionKey">Function key value.</param>
        /// <returns>The user has permission on a specific function or not.</returns>
        public bool HasWritePermission(int userId, string functionKey)
        {
            CachingService cachingService = new CachingService();
            var userCache = cachingService.GetUserCache(userId);
            var policyList = userCache.Functions;
            var foundItems = policyList.Where(x => x.FunctionKey == functionKey);
            if (foundItems.Count() > 0)
            {
                var firstItem = foundItems.First();
                if (firstItem.Policy.Writable)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the user has the permission on a specific function or not.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="functionKey">Function key value.</param>
        /// <returns>The user has permission on a specific function or not.</returns>
        public bool HasReadPermission(int userId, string functionKey)
        {
            CachingService cachingService = new CachingService();
            var userCache = cachingService.GetUserCache(userId);
            var policyList = userCache.Functions;
            var foundItems = policyList.Where(x => x.FunctionKey == functionKey);
            if (foundItems.Count() > 0)
            {
                var firstItem = foundItems.First();
                if (firstItem.Policy.Readable)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Updates user's policy.
        /// </summary>
        /// <param name="adminId">The ID of administrator who updates the user's policy.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="policy">New policy.</param>
        /// <param name="ip">IP of machine which updates user's policy.</param>
        /// <returns>Whether or not the user's policy is updated successfully.</returns>
        public Result UpdateUserPolicy(int adminId, int userId, string policy, string ip)
        {
            return this._usersDataAccess.RMS_UserPolicyUpd(adminId, userId, policy, ip);
        }

        /// <summary>
        /// Gets user cache.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>User Cache.</returns>
        public UserCache GetUserCache(int userId)
        {
            var data = this._usersDataAccess.RMS_MenuSelByUserID(userId);
            var menus = data.Menus.Select(x => new Menu
            {
                Id = x.MenuId,
                Name = x.MenuName,
                Url = x.Url,
                Parent = x.ParentId == 0 ? null : new Menu
                {
                    Id = x.ParentId,
                    Name = x.ParentName
                },
                DisplayOrder = x.DisplayOrder
            });

            var functions = data.Functions.Select(x => new FunctionPolicy
            {
                FunctionId = x.FuncId,
                FunctionName = x.FuncName,
                FunctionKey = x.FuncKey,
                Policy = new Policy(x.Policy)
            });

            return new UserCache(menus, functions);
        }

        /// <summary>
        /// Gets customer information.
        /// </summary>
        /// <param name="username">Username value.</param>
        /// <returns>Customer Information.</returns>
        public RMS_CustomerSelInfoByUsername GetCustomerInfo(string username)
        {
            return this._usersDataAccess.RMS_CustomerSelInfoByUsername(username);
        }
    }
}