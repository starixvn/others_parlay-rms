namespace ParlayRMS.Services.Interface.Users
{
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.Users;

    public interface IUsersService
    {
        User GetUserById(int userId);

        IEnumerable<User> GetListUser(string userName, int groupId);

        Result ResetPassword(int adminId, int userId, string password, string ip);

        Result UpdateStatus(int adminId, int userId, bool active, string ip);

        Result EditUser(int adminId, int userId, string firstName, string lastName, int groupId, string ip);

        Result CreateUser(int adminId, string userName, string password, string firstName, string lastName, int groupId, string ip);

        Result CreateExistUser(int adminId, string userName, int groupId, string ip);

        IEnumerable<Function> GetUserFunction(int userId);

        Result UpdateUserPolicy(int adminId, int userId, string policy, string ip);

        UserCache GetUserCache(int userId);
    }
}