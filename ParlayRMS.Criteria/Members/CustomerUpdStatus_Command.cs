namespace ParlayRMS.Criteria.Members
{
    using Fanex.Data.Repository;

    public class CustomerUpdStatus_Command : NonQueryCommand, IResult
    {
        public int CustId { get; set; }

        public int AdminId { get; set; }

        public bool Monitored { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "RMS_CustomerUpdStatus";

        public override bool IsValid() => true;
    }
}
