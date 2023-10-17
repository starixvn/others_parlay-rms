namespace ParlayRMS.Criteria.Agents
{
    using Fanex.Data.Repository;

    public class DangerousAgentIns_Command: NonQueryCommand, IResult
    {
        public string Username { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "RMS_DangerousAgentIns";

        public override bool IsValid() => true;
    }
}
