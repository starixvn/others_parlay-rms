// --------------------------------------------------------------------------
// <copyright file="CurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Web.Session
{
    using System;
    using System.Web.UI;
    using ParlayRMS.Models.Users;

    /// <summary>
    /// Current Session.
    /// </summary>
    public class CurrentSession : Page, ICurrentSession
    {
        public const string ParlayRmsUsername = "RMS.Username";
        public const string ParlayRmsCustId = "RMS.CustId";
        public const string ParlayRmsUserId = "RMS.UserId";
        public const string ParlayRmsOnUserId = "RMS.OnUserId";
        public const string ParlayRmsUserPolicy = "RMS.UserPolicy";

        public static ICurrentSession Instance
        {
            get
            {
                return new CurrentSession();
            }
        }

        public UserCache UserPolicy
        {
            get
            {
                return (UserCache)this.Session[ParlayRmsUserPolicy];
            }

            set
            {
                this.Session[ParlayRmsUserPolicy] = value;
            }
        }

        public string UserName
        {
            get
            {
                return Convert.ToString(this.Session[ParlayRmsUsername]);
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
                return Convert.ToInt32(this.Session[ParlayRmsCustId]);
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
                return Convert.ToInt32(this.Session[ParlayRmsUserId]);
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
                return Convert.ToInt32(this.Session[ParlayRmsOnUserId]);
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