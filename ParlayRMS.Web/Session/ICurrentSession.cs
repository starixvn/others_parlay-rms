namespace ParlayRMS.Web.Session
{
    using Models.Users;

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
