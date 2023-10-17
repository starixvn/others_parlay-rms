namespace ParlayRMS.Criteria.Agents
{
    using Fanex.Data.Repository;

    public class DangerousAgentDel_Command : NonQueryCommand, IResult
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "RMS_DangerousAgentDel";

        public override bool IsValid() => true;
    }
}
