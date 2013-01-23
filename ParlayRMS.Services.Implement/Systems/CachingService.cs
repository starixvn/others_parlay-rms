namespace ParlayRMS.Services.Implement.Systems
{
    using ParlayRMS.Models.Users;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Web.Session;

    public class CachingService : BaseService
    {
        private readonly IUsersService usersService = new UsersService();
        private readonly ICurrentSession session = CurrentSession.Instance;
        //private readonly ICache userCache = CacheService.CreateInstance(CacheType.MemoryCache);

        public UserCache GetUserCache(int userId)
        {
            //var result = userCache.Get<UserCache>(userId.ToString());
            var result = session.UserPolicy;

            if (result == null || result.IsExpired)
            {
                result = this.usersService.GetUserCache(userId);
                //userCache.Set<UserCache>(userId.ToString(), result, DateTime.Now.AddSeconds(30));
                session.UserPolicy = result;
            }

            return result;
        }
    }
}