namespace ParlayRMS.Services.Systems
{
    using System;
    using ParlayRMS.Models.Users;
    using Web.Session;
    using Users;

    public class CachingService : BaseService, IDisposable
    {
        private readonly UsersService _usersService = new UsersService();
        private readonly CurrentSession _session = new CurrentSession();
        private bool _disposed;

        public UserCache GetUserCache(int userId)
        {
            var result = _session.UserPolicy;

            if (result == null || result.IsExpired)
            {
                result = _usersService.GetUserCache(userId);
                _session.UserPolicy = result;
            }

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _session.Dispose();
                }
            }

            _disposed = true;
        }
    }
}
