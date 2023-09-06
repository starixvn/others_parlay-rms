namespace ParlayRMS.Criteria.ParlayMonitor
{
    using Fanex.Data.Repository;

    public class ComboParlay_PotentialWinning_UserTracking_Command : NonQueryCommand
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string TransId { get; set; }

        public byte StatusId { get; set; }

        public string Message { get; set; }

        public override string GetSettingKey() => "RMS_ComboParlay_PotentialWinning_UserTracking";

        public override bool IsValid() => true;
    }
}
