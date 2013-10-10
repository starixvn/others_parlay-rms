// --------------------------------------------------------------------------
// <copyright file="GroupsService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.Groups
{
    using System.Collections.Generic;
    using System.Linq;
    using ParlayRMS.DataAccess.Implement.Groups;
    using ParlayRMS.DataAccess.Interface.Groups;
    using ParlayRMS.DTO;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.Groups;
    using ParlayRMS.Models.Users;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.Groups;
    using ParlayRMS.Utility;
    using ParlayRMS.Web.Session;

    /// <summary>
    /// Groups Service.
    /// </summary>
    public class GroupsService : BaseService, IGroupsService
    {
        private readonly IGroupsDataAccess _groupDataAccess = new GroupsDataAccess();
        private readonly ICurrentSession _currentSession = CurrentSession.Instance;

        /// <summary>
        /// Gets all groups.
        /// </summary>
        /// <returns>List of Groups.</returns>
        public IEnumerable<Group> GetGroups()
        {
            var data = this._groupDataAccess.RMS_GroupInfoSel();
            return data.Select(x => new Group
            {
                Id = x.GroupId,
                Name = x.GroupName,
                Description = x.Description,
                NumberOfMember = x.NoUsers,
                Creator = new User
                {
                    Name = x.Creator
                },
                CreateDate = x.CreatedDate
            });
        }

        /// <summary>
        /// Gets group by ID.
        /// </summary>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Group associated with the ID.</returns>
        public Group GetGroupById(int groupId)
        {
            var data = this._groupDataAccess.RMS_GroupInfoSelByGroupID(groupId);
            return new Group
            {
                Id = groupId,
                Name = data.GroupName,
                Description = data.Description
            };
        }

        /// <summary>
        /// Creates a new group.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="description">Group description.</param>
        /// <param name="adminId">The ID of administrator who creates the group.</param>
        /// <param name="ip">IP of machine which creates the group.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not group is created successfully.</returns>
        public Result CreateGroup(string groupName, string description, int adminId, string ip, out int groupId)
        {
            return this._groupDataAccess.RMS_GroupInfoIns(groupName, description, adminId, ip, out groupId);
        }

        /// <summary>
        /// Updates a group.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="description">Group description.</param>
        /// <param name="adminId">The ID of administrator who updates the group.</param>
        /// <param name="ip">IP of machine which updates the group.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not group is updated successfully.</returns>
        public Result EditGroup(string groupName, string description, int adminId, string ip, int groupId)
        {
            return this._groupDataAccess.RMS_GroupInfoUpd(groupName, description, adminId, ip, groupId);
        }

        /// <summary>
        /// Gets all groups.
        /// </summary>
        /// <returns>List of Group Information.</returns>
        public IEnumerable<Group> GetGroupNames()
        {
            var data = this._groupDataAccess.RMS_GroupInfoSelAllGroup();
            return data.Select(x => new Group
            {
                Id = x.GroupId,
                Name = x.GroupName
            });
        }

        /// <summary>
        /// Gets list of functions by group ID.
        /// </summary>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>List of functions.</returns>
        public IEnumerable<Function> GetFunctionsByGroupId(int groupId)
        {
            var userService = new UsersService();
            var user = userService.GetUserById(_currentSession.UserId);
            var data = this._groupDataAccess.RMS_GroupPolicySel(groupId);
            if (!AppSetting.HiddenGroups.Contains(user.Group.Name))
            {
                data = data.Where(x => x.FuncKey != FunctionKeys.ManageFunction && x.FuncKey != FunctionKeys.ManageMenu);
            }

            return data.Select(x => new Function
            {
                Id = x.FuncId,
                Name = x.FuncName,
                DisplayOrder = x.DisplayOrder,
                Parent = new Function
                {
                    Id = x.ParentId,
                    Name = x.ParentName
                },
                GroupPolicy = new Policy(x.Policy)
            });
        }

        /// <summary>
        /// Updates policy of a group.
        /// </summary>
        /// <param name="groupPolicy">Group policy.</param>
        /// <param name="adminId">The ID of administrator who updates the group.</param>
        /// <param name="ip">IP of machine which updates the group.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not group policy is updated successfully.</returns>
        public Result EditGroupPolicy(string groupPolicy, int adminId, string ip, int groupId)
        {
            return this._groupDataAccess.RMS_GroupPolicyUpd(groupPolicy, adminId, ip, groupId);
        }
    }
}