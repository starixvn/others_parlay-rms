namespace ParlayRMS.Criteria.Members
{
    using Fanex.Data.Repository;

    public class CustomerIns_Command : NonQueryCommand, IResult
    {
        public string Username { get; set; }

        public int AdminId { get; set; }

        public int Level { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "RMS_CustomerIns";

        public override bool IsValid() => true;
    }
}
