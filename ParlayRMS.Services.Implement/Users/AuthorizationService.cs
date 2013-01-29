namespace ParlayRMS.Services.Implement.Users
{
    using ParlayRMS.DataAccess.Implement;
    using ParlayRMS.DataAccess.Interface;
    using ParlayRMS.Models;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.DataAccess.Interface.Users;
    using ParlayRMS.DataAccess.Implement.Users;
    using ParlayRMS.Common.Models;
    using ParlayRMS.Models.Groups;
    using ParlayRMS.DTO;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Models.Users;

    public class AuthorizationService : BaseService, IAuthorizationService
    {
        private readonly IUsersDataAccess usersDataAccess = new UsersDataAccess();

        public int LoginOnUser(int userId, string sessionId, string ip)
        {
            return this.usersDataAccess.RMS_UserCheckIn(userId, sessionId, ip);
        }

        public void Logout(int userId, string userName)
        {
            if (userId > 0)
            {
                this.usersDataAccess.RMS_UserLogout(userId, userName);
            }
        }

        public bool ReLogin(int userId)
        {
            return this.usersDataAccess.RMS_UserRecheckIn(userId);
        }

        public User ValidateUser(string userName, string passWord, string netSessionId, Request request, out Result result, out int onUserId)
        {
            onUserId = 0;
            var data = this.usersDataAccess.RMS_UserLogin(userName, passWord, request.Ip, request.Domain, request.Browser, request.Platform, request.CountryCode, request.Language, out result);

            if (result.IsSuccessful)
            {
                onUserId = this.LoginOnUser(data.CustId, netSessionId, request.Ip);
                return new User
                {
                    Id = data.CustId,
                    Name = data.UserName,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Group = new Group
                    {
                        Id = data.GroupId,
                        Name = data.GroupName
                    },
                    IsActive = data.Enabled
                };
            }

            return null;
        }
    }
}