namespace ParlayRMS.DTO.BetList
{
    public class RMS_Parlay_TransDetails
    {
        public long TransId { get; set; }

        public int BetType { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public decimal Hdp1 { get; set; }

        public decimal Hdp2 { get; set; }

        public decimal Odds { get; set; }

        public string BetTeam { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }

        public bool LiveIndicator { get; set; }

        public int LeagueId { get; set; }

        public int SportType { get; set; }

        public bool IsNeutral { get; set; }

        public int MatchId { get; set; }

        public int MatchCode { get; set; }

        public string BetStatus { get; set; }
    }
}