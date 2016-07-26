// --------------------------------------------------------------------------
// <copyright file="IGroupsService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.Groups
{
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.Groups;

    public interface IGroupsService
    {
        /// <summary>
        /// Gets All Groups.
        /// </summary>
        /// <returns>List of Groups.</returns>
        IEnumerable<Group> GetGroups();

        /// <summary>
        /// Gets all user groups.
        /// </summary>
        /// <returns>List of Group Information.</returns>
        IEnumerable<Group> GetGroupNames();

        /// <summary>
        /// Gets list of functions by group ID.
        /// </summary>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>List of functions.</returns>
        IEnumerable<Function> GetFunctionsByGroupId(int groupId);

        /// <summary>
        /// Gets a group by ID.
        /// </summary>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Group associated with the ID.</returns>
        Group GetGroupById(int groupId);

        /// <summary>
        /// Creates a new group.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="description">Group description.</param>
        /// <param name="adminId">The ID of administrator who creates the group.</param>
        /// <param name="ip">IP of machine which creates the group.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not group is created successfully.</returns>
        Result CreateGroup(string groupName, string description, int adminId, string ip, out int groupId);

        /// <summary>
        /// Updates a group.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="description">Group description.</param>
        /// <param name="adminId">The ID of administrator who updates the group.</param>
        /// <param name="ip">IP of machine which updates the group.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not group is updated successfully.</returns>
        Result EditGroup(string groupName, string description, int adminId, string ip, int groupId);

        /// <summary>
        /// Updates policy of a group.
        /// </summary>
        /// <param name="groupPolicy">Group policy.</param>
        /// <param name="adminId">The ID of administrator who updates the group.</param>
        /// <param name="ip">IP of machine which updates the group.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not group policy is updated successfully.</returns>
        Result EditGroupPolicy(string groupPolicy, int adminId, string ip, int groupId);
    }
}