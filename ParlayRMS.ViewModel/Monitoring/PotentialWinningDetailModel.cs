namespace ParlayRMS.ViewModel.Monitoring
{
    public class PotentialWinningDetailModel
    {
        public string Combos { get; set; }

        public string ParlayTypes { get; set; }

        public string SportsTypes { get; set; }

        public long CustId { get; set; }

        public string UserName { get; set; }

        public bool IsCornerParlay { get; set; }

        public int? AgentId { get; set; }
    }
}