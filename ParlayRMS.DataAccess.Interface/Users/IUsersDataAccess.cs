namespace ParlayRMS.DataAccess.Interface.Users
{
    using System.Collections.Generic;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DTO.Users;
    using ParlayRMS.DTO;

    public interface IUsersDataAccess
    {
        IEnumerable<RMS_UserInfoSel> RMS_UserInfoSel(string userName, int groupId);

        Result RMS_UserInfoUpdUserPwd(int adminId, int userId, string password, string ip);

        Result RMS_UserInfoAddNew(int adminId, string userName, string password, string firstName, string lastName, int groupId, string ip);

        Result RMS_UserInfoUpdAll(int adminId, int userId, string firstName, string lastName, string ip, int groupId);

        Result RMS_UserInfoUpdStatus(int adminId, int userId, bool active, string ip);

        Result RMS_UserInfoAddExisting(int adminId, string userName, int groupId, string ip);

        IEnumerable<RMS_UserPolicySel> RMS_UserPolicySel(int userId);

        Result RMS_UserPolicyUpd(int adminId, int userId, string policy, string ip);

        RMS_MenuSelByUserID RMS_MenuSelByUserID(int userId);

        RMS_UserInfoSelByUserID RMS_UserInfoSelByUserID(int userId);

        RMS_UserInfoSelByUserID RMS_UserLogin(string userName, string password, string loginIp, string loginDomain, string browserName, string platform, string countryCode, string language, out Result result);

        bool RMS_UserRecheckIn(int userId);

        int RMS_UserCheckIn(int userId, string netSessionId, string ip);

        void RMS_UserLogout(int userId, string userName);

        RMS_CustomerSelInfoByUsername RMS_CustomerSelInfoByUsername(string username);
    }
}