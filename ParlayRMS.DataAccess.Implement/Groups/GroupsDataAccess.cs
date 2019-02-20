// --------------------------------------------------------------------------
// <copyright file="GroupsDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement.Groups
{
    using System.Collections;
    using System.Collections.Generic;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Groups;
    using ParlayRMS.Utility;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Group Data Access.
    /// </summary>
    public class GroupsDataAccess : BaseDataAccess
    {
        /// <summary>
        /// Gets all user groups.
        /// </summary>
        /// <returns>List of Group Information.</returns>
        public IEnumerable<RMS_GroupInfoSel> RMS_GroupInfoSel()
        {
            var result = new List<RMS_GroupInfoSel>();

            using (var reader = ExecSpDataReader("RMS_GroupInfoSel"))
            {
                while (reader.Read())
                {
                    result.Add(new RMS_GroupInfoSel
                    {
                        GroupId = reader.ToInt32("groupid"),
                        GroupName = reader.ToString("groupname"),
                        NoUsers = reader.ToInt32("nousers"),
                        Description = reader.ToString("description"),
                        Creator = reader.ToString("creator"),
                        CreatedDate = reader.ToDateTime("createddate")
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Gets group by ID.
        /// </summary>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Group associated with the ID.</returns>
        public RMS_GroupInfoSelByGroupID RMS_GroupInfoSelByGroupID(int groupId)
        {
            Hashtable param = new Hashtable();
            param.Add("groupid", groupId);
            using (var reader = ExecSpDataReader("RMS_GroupInfoSelByGroupID", param))
            {
                while (reader.Read())
                {
                    return new RMS_GroupInfoSelByGroupID
                    {
                        GroupName = reader.ToString("groupname"),
                        Description = reader.ToString("Description")
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// Creates new group.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="description">Group description.</param>
        /// <param name="adminId">The ID of administrator who creates the group.</param>
        /// <param name="ip">IP of machine which creates the group.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not group is created successfully.</returns>
        public Result RMS_GroupInfoIns(string groupName, string description, int adminId, string ip, out int groupId)
        {
            groupId = 0;
            var result = Result.Empty;

            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("groupname", groupName);
            param.Add("description", description);
            param.Add("ip", ip);
            param.Add("groupid", 0);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            var r = ExecSp("RMS_GroupInfoIns", param);
            result = r.ToResult();
            if (result.IsSuccessful)
            {
                groupId = DbConverter.ToInt32(r.Outputs["groupid"]);
            }

            return result;
        }

        /// <summary>
        /// Updates an existing group.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="description">Group description.</param>
        /// <param name="adminId">The ID of administrator who updates the group.</param>
        /// <param name="ip">IP of machine which updates the group.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not group is updated successfully.</returns>
        public Result RMS_GroupInfoUpd(string groupName, string description, int adminId, string ip, int groupId)
        {
            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("groupid", groupId);
            param.Add("groupname", groupName);
            param.Add("description", description);
            param.Add("ip", ip);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            var r = ExecSp("RMS_GroupInfoUpd", param);

            return r.ToResult();
        }

        /// <summary>
        /// Gets all user groups.
        /// </summary>
        /// <returns>List of Group Information.</returns>
        public IEnumerable<RMS_GroupInfoSelAllGroup> RMS_GroupInfoSelAllGroup()
        {
            var result = new List<RMS_GroupInfoSelAllGroup>();

            using (var reader = ExecSpDataReader("RMS_GroupInfoSelAllGroup"))
            {
                while (reader.Read())
                {
                    result.Add(new RMS_GroupInfoSelAllGroup
                    {
                        GroupId = reader.ToInt32("groupid"),
                        GroupName = reader.ToString("groupname"),
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Gets group policy.
        /// </summary>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Policy of group of given ID.</returns>
        public IEnumerable<RMS_GroupPolicySel> RMS_GroupPolicySel(int groupId)
        {
            var result = new List<RMS_GroupPolicySel>();

            Hashtable param = new Hashtable();
            param.Add("groupid", groupId);
            using (var reader = ExecSpDataReader("RMS_GroupPolicySel", param))
            {
                while (reader.Read())
                {
                    result.Add(new RMS_GroupPolicySel
                    {
                        FuncId = reader.ToInt32("funcid"),
                        FuncKey = reader.ToString("funckey"),
                        FuncName = reader.ToString("funcname"),
                        DisplayOrder = reader.ToInt32("displayorder"),
                        ParentId = reader.ToInt32("parentid"),
                        ParentName = reader.ToString("parentname"),
                        Policy = reader.ToInt32("policy")
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Updates policy of a group.
        /// </summary>
        /// <param name="groupPolicy">Group policy.</param>
        /// <param name="adminId">The ID of administrator who updates the group.</param>
        /// <param name="ip">IP of machine which updates the group.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not group policy is updated successfully.</returns>
        public Result RMS_GroupPolicyUpd(string groupPolicy, int adminId, string ip, int groupId)
        {
            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("groupid", groupId);
            param.Add("grouppolicy", groupPolicy);
            param.Add("ip", ip);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            var r = ExecSp("RMS_GroupPolicyUpd", param);

            return r.ToResult();
        }
    }
}