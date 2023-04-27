namespace ParlayRMS.DTO.LeagueWinningMonitor
{
    public class RmsRptLeagueWinningMonitorDetail
    {
        public string TransDate { get; set; }

        public long TransId { get; set; }

        public string Username { get; set; }

        public string Level { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal WinningAmount { get; set; }

        public decimal CompanyLoseAmount { get; set; }

        public int Combo { get; set; }

        public int TotalSelection { get; set; }

        public int SportType { get; set; }

        public string SportTypeName { get; set; }

        public string IP { get; set; }

        public int BetType { get; set; }

        public long RefNo { get; set; }

        public int CustId { get; set; }

        public string SiteGroup { get; set; }
    }
}