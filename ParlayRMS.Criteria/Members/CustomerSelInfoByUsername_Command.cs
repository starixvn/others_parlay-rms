namespace ParlayRMS.Criteria.Members
{
    using Fanex.Data.Repository;

    public class CustomerSelInfoByUsername_Command : NonQueryCommand
    {
        public string Username { get; set; }

        public int CustId { get; set; }

        public int RoleId { get; set; }

        public override string GetSettingKey() => "RMS_CustomerSelInfoByUsername";

        public override bool IsValid() => true;
    }
}
