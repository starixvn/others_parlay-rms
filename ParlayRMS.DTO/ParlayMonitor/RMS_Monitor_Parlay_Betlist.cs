using System;

namespace ParlayRMS.DTO.ParlayMonitor
{
    public class RMS_Monitor_Parlay_Betlist
    {
        public DateTime TransDate { get; set; }

        public string TransDateString { get; set; }

        public string TransId { get; set; }

        public long RefNo { get; set; }

        public string Username { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal PotentialWinningAmount { get; set; }

        public decimal NetWinAfterPT { get; set; }

        public int TotalSelection { get; set; }

        public int TotalBet { get; set; }

        public int NumberWon { get; set; }

        public string IP { get; set; }

        public int Level { get; set; }

        public bool IsBetRadar { get; set; }

        public int SportType { get; set; }

        public string SportTypeName { get; set; }

        public int BetType { get; set; }

        public int CustId { get; set; }

        public bool IsLive { get; set; }

        public bool IsBlacklistLeague { get; set; }

        public bool IsGOF { get; set; }

        public string CustomerClass { get; set; }

        public string TrackingUser { get; set; }

        public bool IsTrackedUser { get; set; }

        public string SiteGroup { get; set; }
    }
}