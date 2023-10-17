namespace ParlayRMS.Criteria.Leagues
{
    using Fanex.Data.Repository;

    public class LeagueUpd_Command : NonQueryCommand, IResult
    {
        public int LeagueId { get; set; }

        public int GroupId { get; set; }

        public int AdminId { get; set; }

        public string Ip { get; set; }

        public override string GetSettingKey() => "RMS_LeagueUpd";

        public override bool IsValid() => true;

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}