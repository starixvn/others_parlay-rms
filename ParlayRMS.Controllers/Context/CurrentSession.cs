namespace ParlayRMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using ParlayRMS.Utility;

    public class CurrentSession : Page, ICurrentSession
    {
        #region Constants

        public const string ParlayRmsUsername = "ParlayRMSUsername";
        public const string ParlayRmsCustId = "ParlayRMSCustId";
        public const string ParlayRmsUserId = "ParlayRMSUserId";
        public const string ParlayRmsOnUserId = "ParlayRMSOnUserId";

        #endregion Constants

        private static ICurrentSession instance = null;

        public static ICurrentSession Instance
        {
            get
            {
                return instance ?? new CurrentSession();
            }
        }

        public string UserName
        {
            get
            {
                return Utils.ToString(this.Session[ParlayRmsUsername]);
            }

            set
            {
                this.Session[ParlayRmsUsername] = value;
            }
        }

        public int CustId
        {
            get
            {
                return Utils.ToInt32(this.Session[ParlayRmsCustId]);
            }

            set
            {
                this.Session[ParlayRmsCustId] = value;
            }
        }

        public int UserId
        {
            get
            {
                return Utils.ToInt32(this.Session[ParlayRmsUserId]);
            }

            set
            {
                this.Session[ParlayRmsUserId] = value;
            }
        }

        public int OnUserId
        {
            get
            {
                return Utils.ToInt32(this.Session[ParlayRmsOnUserId]);
            }

            set
            {
                this.Session[ParlayRmsOnUserId] = value;
            }
        }

        public bool IsLogin
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.UserName);
            }
        }

        public string NetSessionId
        {
            get { return this.Session.SessionID; }
        }
    }
}