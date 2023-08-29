using ParlayRMS.Models.Users;
using System;
using System.Web.UI;

namespace ParlayRMS.Web.Session
{
    public class CurrentSession : Page, ICurrentSession
    {
        public const string ParlayRmsUsername = "RMS.Username";
        public const string ParlayRmsCustId = "RMS.CustId";
        public const string ParlayRmsUserId = "RMS.UserId";
        public const string ParlayRmsOnUserId = "RMS.OnUserId";
        public const string ParlayRmsUserPolicy = "RMS.UserPolicy";

        public static ICurrentSession Instance => new CurrentSession();

        public UserCache UserPolicy
        {
            get => (UserCache)Session[ParlayRmsUserPolicy];

            set => Session[ParlayRmsUserPolicy] = value;
        }

        public string UserName
        {
            get => Convert.ToString(Session[ParlayRmsUsername]);

            set => Session[ParlayRmsUsername] = value;
        }

        public int CustId
        {
            get => Convert.ToInt32(Session[ParlayRmsCustId]);

            set => Session[ParlayRmsCustId] = value;
        }

        public int UserId
        {
            get => Convert.ToInt32(Session[ParlayRmsUserId]);

            set => Session[ParlayRmsUserId] = value;
        }

        public int OnUserId
        {
            get => Convert.ToInt32(Session[ParlayRmsOnUserId]);

            set => Session[ParlayRmsOnUserId] = value;
        }

        public bool IsLogin => !string.IsNullOrWhiteSpace(UserName);

        public string NetSessionId => Session.SessionID;
    }
}
