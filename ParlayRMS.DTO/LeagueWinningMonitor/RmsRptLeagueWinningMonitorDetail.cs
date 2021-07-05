using System;

namespace ParlayRMS.DTO.LeagueWinningMonitor
{
    public class RmsRptLeagueWinningMonitorDetail
    {
        public DateTime TransDate { get; set; }

        public long TransId { get; set; }

        public string Username { get; set; }

        public byte Level { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal WinningAmount { get; set; }

        public decimal NetWinAfterPT { get; set; }

        public decimal CompanyLoseAmount { get; set; }

        public int Combo { get; set; }

        public decimal TotalSelection { get; set; }

        public long SportType { get; set; }

        public string IP { get; set; }

        public int BetType { get; set; }

        public long RefNo { get; set; }

        public int CustId { get; set; }
    }
}