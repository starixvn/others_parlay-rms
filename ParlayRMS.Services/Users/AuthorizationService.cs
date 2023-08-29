namespace ParlayRMS.Services.Users
{
    using DataAccess.Users;
    using DTO;
    using ParlayRMS.Models.Groups;
    using ParlayRMS.Models.Users;

    public class AuthorizationService : BaseService
    {
        private readonly UsersDataAccess _usersDataAccess = new UsersDataAccess();

        public int LoginOnUser(int userId, string sessionId, string ip)
        {
            return _usersDataAccess.RMS_UserCheckIn(userId, sessionId, ip);
        }

        public void Logout(int userId, string userName)
        {
            if (userId > 0)
            {
                _usersDataAccess.RMS_UserLogout(userId, userName);
            }
        }

        public bool ReLogin(int userId)
        {
            return _usersDataAccess.RMS_UserRecheckIn(userId);
        }

        public User ValidateUser(string userName, string passWord, string netSessionId, Request request, out Result result, out int userId)
        {
            userId = 0;
            var data = _usersDataAccess.RMS_UserLogin(
                                                 userName,
                                                 passWord,
                                                 request.Ip,
                                                 request.Domain,
                                                 request.Browser,
                                                 request.Platform,
                                                 request.CountryCode,
                                                 request.Language,
                                                 out result);

            if (result.IsSuccessful)
            {
                userId = LoginOnUser(data.CustId, netSessionId, request.Ip);
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
