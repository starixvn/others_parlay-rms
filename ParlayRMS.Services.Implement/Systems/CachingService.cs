namespace ParlayRMS.Services.Implement.Systems
{
    using ParlayRMS.Models.Users;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Web.Session;

    public class CachingService : BaseService
    {
        private readonly IUsersService usersService = new UsersService();
        private readonly ICurrentSession session = new CurrentSession();

        public UserCache GetUserCache(int userId)
        {
            var result = session.UserPolicy;

            if (result == null || result.IsExpired)
            {
                result = this.usersService.GetUserCache(userId);
                session.UserPolicy = result;
            }

            return result;
        }
    }
}