// --------------------------------------------------------------------------
// <copyright file="CachingService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Systems
{
    using System;
    using ParlayRMS.Models.Users;
    using ParlayRMS.Web.Session;
    using Users;

    /// <summary>
    /// Caching Service.
    /// </summary>
    public class CachingService : BaseService, IDisposable
    {
        private readonly UsersService _usersService = new UsersService();
        private readonly CurrentSession _session = new CurrentSession();
        private bool _disposed = false;

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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    (this._session as CurrentSession).Dispose();
                }
            }

            this._disposed = true;
        }
    }
}