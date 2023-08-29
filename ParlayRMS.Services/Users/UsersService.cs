namespace ParlayRMS.Services.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Users;
    using Groups;
    using Models.Functions;
    using DTO;
    using ParlayRMS.DTO.Users;
    using ParlayRMS.Models.Groups;
    using ParlayRMS.Models.Menus;
    using ParlayRMS.Models.Users;
    using Utility;
    using Web.Session;
    using Systems;

    public class UsersService : BaseService
    {
        private readonly UsersDataAccess _usersDataAccess = new UsersDataAccess();
        private readonly GroupsService _groupsService = new GroupsService();
        private readonly ICurrentSession _currentSession = CurrentSession.Instance;

        public User GetUserById(int userId)
        {
            var data = _usersDataAccess.RMS_UserInfoSelByUserID(userId);
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

        public IEnumerable<User> GetListUser(string userName, int groupId)
        {
            var data = _usersDataAccess.RMS_UserInfoSel(userName, groupId);
            return data.Select(x => new User
            {
                Id = x.CustId,
                Name = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Group = _groupsService.GetGroupById(x.GroupId),
                Creator = new User
                {
                    Name = x.Creator
                },
                CreatedDate = x.CreatedDate,
                IsActive = x.Enabled
            });
        }

        public Result ResetPassword(int adminId, int userId, string password, string ip)
        {
            return _usersDataAccess.RMS_UserInfoUpdUserPwd(adminId, userId, password, ip);
        }

        public Result UpdateStatus(int adminId, int userId, bool active, string ip)
        {
            return _usersDataAccess.RMS_UserInfoUpdStatus(adminId, userId, active, ip);
        }

        public Result EditUser(int adminId, int userId, string firstName, string lastName, int groupId, string ip)
        {
            return _usersDataAccess.RMS_UserInfoUpdAll(adminId, userId, firstName, lastName, ip, groupId);
        }

        public Result CreateUser(int adminId, string userName, string password, string firstName, string lastName, int groupId, string ip)
        {
            return _usersDataAccess.RMS_UserInfoAddNew(adminId, userName, password, firstName, lastName, groupId, ip);
        }

        public Result CreateExistUser(int adminId, string userName, int groupId, string ip)
        {
            return _usersDataAccess.RMS_UserInfoAddExisting(adminId, userName, groupId, ip);
        }

        public IEnumerable<Function> GetUserFunction(int userId)
        {
            var user = GetUserById(_currentSession.UserId);
            var data = _usersDataAccess.RMS_UserPolicySel(userId);
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

        public bool HasWritePermission(int userId, string functionKey)
        {
            var cachingService = new CachingService();
            var userCache = cachingService.GetUserCache(userId);
            var policyList = userCache.Functions;
            var foundItems = policyList.Where(x => x.FunctionKey == functionKey);
            if (foundItems.Any())
            {
                var firstItem = foundItems.First();
                if (firstItem.Policy.Writable)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasReadPermission(int userId, string functionKey)
        {
            var cachingService = new CachingService();
            var userCache = cachingService.GetUserCache(userId);
            var policyList = userCache.Functions;
            var foundItems = policyList.Where(x => x.FunctionKey == functionKey);
            if (foundItems.Any())
            {
                var firstItem = foundItems.First();
                if (firstItem.Policy.Readable)
                {
                    return true;
                }
            }

            return false;
        }

        public Result UpdateUserPolicy(int adminId, int userId, string policy, string ip)
        {
            return _usersDataAccess.RMS_UserPolicyUpd(adminId, userId, policy, ip);
        }

        public UserCache GetUserCache(int userId)
        {
            var data = _usersDataAccess.RMS_MenuSelByUserID(userId);
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

        public RMS_CustomerSelInfoByUsername GetCustomerInfo(string username)
        {
            return _usersDataAccess.RMS_CustomerSelInfoByUsername(username);
        }

        public List<int> GetAgents(int masterId)
        {
            return _usersDataAccess.RMS_CustomerAgentOfMasterSel(masterId).ToList();
        }
    }
}
