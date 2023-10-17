namespace ParlayRMS.Criteria.Leagues
{
    using Fanex.Data.Repository;

    public class LeagueIns_Command : NonQueryCommand, IResult
    {
        public string LeagueName { get; set; }

        public int GroupId { get; set; }

        public int AdminId { get; set; }

        public string Ip { get; set; }

        public override string GetSettingKey() => "RMS_LeagueIns";

        public override bool IsValid() => true;

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}