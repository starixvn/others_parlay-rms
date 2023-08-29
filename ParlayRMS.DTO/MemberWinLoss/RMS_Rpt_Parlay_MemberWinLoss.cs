namespace ParlayRMS.DTO.MemberWinLoss
{
    public class RMS_Rpt_Parlay_MemberWinLoss
    {
        public string Month { get; set; }

        public int Tickets_Won { get; set; }

        public int Tickets_Lose { get; set; }

        public decimal Stake_Won { get; set; }

        public decimal Stake_Lose { get; set; }

        public decimal Member_Won { get; set; }

        public decimal Member_Lose { get; set; }

        public decimal OurPT_Won { get; set; }

        public decimal OurPT_Lose { get; set; }

        public decimal OurPT_NetWinloss { get; set; }

        public string Username { get; set; }

        public bool IsSutotalRow { get; set; }

        public int NumOfSubrows { get; set; }

        public string CustomerClass { get; set; }

        public string SiteGroup { get; set; }

        public void Add(RMS_Rpt_Parlay_MemberWinLoss obj)
        {
            Tickets_Won += obj.Tickets_Won;
            Tickets_Lose += obj.Tickets_Lose;
            Stake_Won += obj.Stake_Won;
            Stake_Lose += obj.Stake_Lose;
            Member_Won += obj.Member_Won;
            Member_Lose += obj.Member_Lose;
            OurPT_Won += obj.OurPT_Won;
            OurPT_Lose += obj.OurPT_Lose;
            OurPT_NetWinloss += obj.OurPT_NetWinloss;
            NumOfSubrows += 1;
        }
    }
}
