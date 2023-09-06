namespace ParlayRMS.Criteria.Members
{
    using Fanex.Data.Repository;

    public class CustomerCheckExisting_Command : NonQueryCommand
    {
        public int CustId { get; set; }

        public bool IsExists { get; set; }

        public override string GetSettingKey() => "RMS_CustomerCheckExisting";

        public override bool IsValid() => true;
    }
}
