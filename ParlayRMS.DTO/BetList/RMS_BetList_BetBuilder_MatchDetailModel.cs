using System;

namespace ParlayRMS.DTO.BetList
{
    public class RMS_BetList_BetBuilder_MatchDetailModel
    {
        public long TransId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public bool IsNeutral { get; set; }

        public int SportType { get; set; }

        public int LeagueId { get; set; }

        public DateTime EventDate { get; set; }

        public bool IsBlacklistLeague { get; set; }
    }
}
