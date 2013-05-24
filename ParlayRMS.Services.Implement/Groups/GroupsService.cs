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
    using ParlayRMS.Services.Interface.Groups;
    using ParlayRMS.Web.Session;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Utility;

    public class GroupsService : BaseService, IGroupsService
    {
        private readonly IGroupsDataAccess groupDataAccess = new GroupsDataAccess();
        private readonly ICurrentSession currentSession = CurrentSession.Instance;

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
            var userService = new UsersService();
            var user = userService.GetUserById(currentSession.UserId);
            var data = this.groupDataAccess.RMS_GroupPolicySel(groupId);
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

        public Result EditGroupPolicy(string groupPolicy, int adminId, string ip, int groupId)
        {
            return this.groupDataAccess.RMS_GroupPolicyUpd(groupPolicy, adminId, ip, groupId);
        }
    }
}