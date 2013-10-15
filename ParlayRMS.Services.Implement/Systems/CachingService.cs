// --------------------------------------------------------------------------
// <copyright file="CachingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.Systems
{
    using System;
    using ParlayRMS.Models.Users;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Web.Session;

    /// <summary>
    /// Caching Service.
    /// </summary>
    public class CachingService : BaseService, IDisposable
    {
        private readonly IUsersService _usersService = new UsersService();
        private readonly ICurrentSession _session = new CurrentSession();

        /// <summary>
        /// Gets a user's cached data.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>User cached data.</returns>
        public UserCache GetUserCache(int userId)
        {
            var result = _session.UserPolicy;

            if (result == null || result.IsExpired)
            {
                result = this._usersService.GetUserCache(userId);
                _session.UserPolicy = result;
            }

            return result;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            (this._session as CurrentSession).Dispose();
        }
    }
}