// --------------------------------------------------------------------------
// <copyright file="UserPolicy.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Web.Authorization
{
    public enum UserPolicy : int
    {
        /// <summary>
        /// Has permission on creating.
        /// </summary>
        Create = 4,

        /// <summary>
        /// Has permission on reading.
        /// </summary>
        Read = 1,

        /// <summary>
        /// Has permission on updating.
        /// </summary>
        Update = 2,

        /// <summary>
        /// Has permission on deleting.
        /// </summary>
        Delete = 8
    }
}