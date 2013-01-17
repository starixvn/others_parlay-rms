namespace ParlayRMS.Services.Implement.Systems
{
    using System;
    using Fanex.Caching;
    using ParlayRMS.DataAccess.Implement.Users;
    using ParlayRMS.DataAccess.Interface.Users;
    using ParlayRMS.Models.Users;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Services.Implement.Users;

    public class CachingService : BaseService
    {
        private static readonly ICache userCache = CacheService.CreateInstance(CacheType.MemoryCache);
        private readonly IUsersService usersService = new UsersService();

        public UserCache GetUserCache(int userId)
        {
            var result = userCache.Get<UserCache>(userId.ToString());

            if (result == null)
            {
                result = this.usersService.GetUserCache(userId);
                if (result != null)
                {
                    userCache.Set<UserCache>(userId.ToString(), result, DateTime.Now.AddSeconds(30));
                }
            }

            return result;
        }

        internal void RemoveUserCache(int userId)
        {
            userCache.Remove(userId.ToString());
        }

        internal void ClearUserCache()
        {
            userCache.Clear();
        }
    }
}