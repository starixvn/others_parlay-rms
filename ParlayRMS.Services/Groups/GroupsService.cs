namespace ParlayRMS.Services.Groups
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Groups;
    using Models.Functions;
    using DTO;
    using ParlayRMS.Models.Groups;
    using ParlayRMS.Models.Users;
    using Utility;
    using Web.Session;
    using Users;

    public class GroupsService : BaseService
    {
        private readonly GroupsDataAccess _groupDataAccess = new GroupsDataAccess();
        private readonly ICurrentSession _currentSession = CurrentSession.Instance;

        public IEnumerable<Group> GetGroups()
        {
            var data = _groupDataAccess.RMS_GroupInfoSel();
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
            var data = _groupDataAccess.RMS_GroupInfoSelByGroupID(groupId);
            return new Group
            {
                Id = groupId,
                Name = data.GroupName,
                Description = data.Description
            };
        }

        public Result CreateGroup(string groupName, string description, int adminId, string ip, out int groupId)
        {
            return _groupDataAccess.RMS_GroupInfoIns(groupName, description, adminId, ip, out groupId);
        }

        public Result EditGroup(string groupName, string description, int adminId, string ip, int groupId)
        {
            return _groupDataAccess.RMS_GroupInfoUpd(groupName, description, adminId, ip, groupId);
        }

        public IEnumerable<Group> GetGroupNames()
        {
            var data = _groupDataAccess.RMS_GroupInfoSelAllGroup();
            return data.Select(x => new Group
            {
                Id = x.GroupId,
                Name = x.GroupName
            });
        }

        public IEnumerable<Function> GetFunctionsByGroupId(int groupId)
        {
            var userService = new UsersService();
            var user = userService.GetUserById(_currentSession.UserId);
            var data = _groupDataAccess.RMS_GroupPolicySel(groupId);
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
            return _groupDataAccess.RMS_GroupPolicyUpd(groupPolicy, adminId, ip, groupId);
        }
    }
}
