namespace ParlayRMS.Services.Interface.Users
{
    using ParlayRMS.DTO;
    using ParlayRMS.Models.Users;

    public interface IAuthorizationService
    {
        int LoginOnUser(int userId, string sessionId, string ip);

        void Logout(int userId, string userName);

        bool ReLogin(int userId);

        User ValidateUser(string userName, string passWord, string netSessionId, Request request, out Result result, out int onUserId);
    }
}