namespace ParlayRMS.Services.Implement.Groups
{
    using System.Linq;
    using System.Collections.Generic;
    using Fanex.Logging;
    using ParlayRMS.DataAccess.Implement;
    using ParlayRMS.DataAccess.Interface;
    using ParlayRMS.Models;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Interface.Groups;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.DataAccess.Interface.Groups;
    using ParlayRMS.DataAccess.Implement.Groups;
    using ParlayRMS.Common.Models;
    using ParlayRMS.Models.Groups;
    using ParlayRMS.DTO;
    using ParlayRMS.Services.Implement.Systems;
    using ParlayRMS.Models.Users;

    public class GroupsService : BaseService, IGroupsService
    {
        private readonly IGroupsDataAccess groupDataAccess = new GroupsDataAccess();

        public IEnumerable<Group> GetGroups()
        {
            var data = this.groupDataAccess.RMS_GroupInfoSel();
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

        public Group GetGroupById(int groupId)
        {
            var data = this.groupDataAccess.RMS_GroupInfoSelByGroupID(groupId);
            return new Group
            {
                Id = groupId,
                Name = data.GroupName,
                Description = data.Description
            };
        }

        public Result CreateGroup(string groupName, string description, int adminId, string ip, out int groupId)
        {
            return this.groupDataAccess.RMS_GroupInfoIns(groupName, description, adminId, ip, out groupId);
        }

        public Result EditGroup(string groupName, string description, int adminId, string ip, int groupId)
        {
            return this.groupDataAccess.RMS_GroupInfoUpd(groupName, description, adminId, ip, groupId);
        }

        public IEnumerable<Group> GetGroupNames()
        {
            var data = this.groupDataAccess.RMS_GroupInfoSelAllGroup();
            return data.Select(x => new Group
            {
                Id = x.GroupId,
                Name = x.GroupName
            });
        }

        public IEnumerable<Function> GetFunctionsByGroupId(int groupId)
        {
            var data = this.groupDataAccess.RMS_GroupPolicySel(groupId);
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

        public Result EditGroupPolicy(string groupPolicy, int adminId, string ip, int groupId)
        {
            return this.groupDataAccess.RMS_GroupPolicyUpd(groupPolicy, adminId, ip, groupId);
        }
    }
}