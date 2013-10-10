// --------------------------------------------------------------------------
// <copyright file="ICurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Web.Session
{
    using ParlayRMS.Models.Users;

    public interface ICurrentSession
    {
        UserCache UserPolicy { get; set; }

        string UserName { get; set; }

        int CustId { get; set; }

        int UserId { get; set; }

        bool IsLogin { get; }

        int OnUserId { get; set; }

        string NetSessionId { get; }
    }
}