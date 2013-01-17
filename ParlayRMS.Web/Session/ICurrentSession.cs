namespace ParlayRMS.Web.Session
{
    public interface ICurrentSession
    {
        string UserName { get; set; }

        int CustId { get; set; }

        int UserId { get; set; }

        bool IsLogin { get; }

        int OnUserId { get; set; }

        string NetSessionId { get; }
    }
}