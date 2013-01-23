namespace ParlayRMS.Services.Implement.Users
{
    using System.Linq;
    using System.Collections.Generic;
    using ParlayRMS.DataAccess.Implement.Users;
    using ParlayRMS.DataAccess.Interface.Users;
    using ParlayRMS.Models;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Interface.Groups;
    using ParlayRMS.Services.Implement.Groups;
    using ParlayRMS.Models.Users;
    using ParlayRMS.Models.Menus;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DTO;
    using ParlayRMS.Services.Implement.Systems;

    public class UsersService : BaseService, IUsersService
    {
        private readonly IUsersDataAccess usersDataAccess = new UsersDataAccess();
        private readonly IGroupsService groupsService = new GroupsService();

        public IEnumerable<User> GetListUser(string userName, int groupId)
        {
            var data = this.usersDataAccess.RMS_UserInfoSel(userName, groupId);
            return data.Select(x => new User
            {
                Id = x.CustId,
                Name = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Group = this.groupsService.GetGroupById(x.GroupId),
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
            return this.usersDataAccess.RMS_UserInfoUpdUserPwd(adminId, userId, password, ip);
        }

        public Result UpdateStatus(int adminId, int userId, bool active, string ip)
        {
            return this.usersDataAccess.RMS_UserInfoUpdStatus(adminId, userId, active, ip);
        }

        public Result EditUser(int adminId, int userId, string firstName, string lastName, int groupId, string ip)
        {
            return this.usersDataAccess.RMS_UserInfoUpdAll(adminId, userId, firstName, lastName, ip, groupId);
        }

        public Result CreateUser(int adminId, string userName, string password, string firstName, string lastName, int groupId, string ip)
        {
            return this.usersDataAccess.RMS_UserInfoAddNew(adminId, userName, password, firstName, lastName, groupId, ip);
        }

        public Result CreateExistUser(int adminId, string userName, int groupId, string ip)
        {
            return this.usersDataAccess.RMS_UserInfoAddExisting(adminId, userName, groupId, ip);
        }

        public IEnumerable<Function> GetUserFunction(int userId)
        {
            var data = this.usersDataAccess.RMS_UserPolicySel(userId);
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

        public Result UpdateUserPolicy(int adminId, int userId, string policy, string ip)
        {
            return this.usersDataAccess.RMS_UserPolicyUpd(adminId, userId, policy, ip);
        }

        public UserCache GetUserCache(int userId)
        {
            var data = this.usersDataAccess.RMS_MenuSelByUserID(userId);
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
    }
}