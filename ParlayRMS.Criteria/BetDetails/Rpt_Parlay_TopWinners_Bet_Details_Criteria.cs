namespace ParlayRMS.Criteria.BetDetails
{
    using Fanex.Data.Repository;

    public  class Rpt_Parlay_TopWinners_Bet_Details_Criteria: CriteriaBase
    {
        public long TransId { get; set; }

        public long RefNo { get; set; }

        public override string GetSettingKey() => "RMS_Rpt_Parlay_TopWinners_Bet_Details";

        public override bool IsValid() => true;
    }
}
