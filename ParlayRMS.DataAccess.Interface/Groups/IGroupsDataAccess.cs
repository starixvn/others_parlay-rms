// --------------------------------------------------------------------------
// <copyright file="IGroupsDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.Groups
{
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Groups;

    public interface IGroupsDataAccess
    {
        /// <summary>
        /// Gets all user groups.
        /// </summary>
        /// <returns>List of Group Information.</returns>
        IEnumerable<RMS_GroupInfoSel> RMS_GroupInfoSel();

        /// <summary>
        /// Gets all user groups.
        /// </summary>
        /// <returns>List of Group Information.</returns>
        IEnumerable<RMS_GroupInfoSelAllGroup> RMS_GroupInfoSelAllGroup();

        /// <summary>
        /// Gets a group policy.
        /// </summary>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Policy of group of given ID.</returns>
        IEnumerable<RMS_GroupPolicySel> RMS_GroupPolicySel(int groupId);

        /// <summary>
        /// Gets a group by ID.
        /// </summary>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Group associated with the ID.</returns>
        RMS_GroupInfoSelByGroupID RMS_GroupInfoSelByGroupID(int groupId);

        /// <summary>
        /// Creates a new group.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="description">Group description.</param>
        /// <param name="adminId">The ID of administrator who creates the group.</param>
        /// <param name="ip">IP of machine which creates the group.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not group is created successfully.</returns>
        Result RMS_GroupInfoIns(string groupName, string description, int adminId, string ip, out int groupId);

        /// <summary>
        /// Updates a group information.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="description">Group description.</param>
        /// <param name="adminId">The ID of administrator who updates the group.</param>
        /// <param name="ip">IP of machine which updates the group.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not group is updated successfully.</returns>
        Result RMS_GroupInfoUpd(string groupName, string description, int adminId, string ip, int groupId);

        /// <summary>
        /// Updates policy of a group.
        /// </summary>
        /// <param name="groupPolicy">Group policy.</param>
        /// <param name="adminId">The ID of administrator who updates the group.</param>
        /// <param name="ip">IP of machine which updates the group.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not group policy is updated successfully.</returns>
        Result RMS_GroupPolicyUpd(string groupPolicy, int adminId, string ip, int groupId);
    }
}