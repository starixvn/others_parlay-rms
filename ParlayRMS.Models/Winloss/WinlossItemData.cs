using ParlayRMS.DTO.Winloss;
using System.Collections.Generic;

namespace ParlayRMS.Models.Winloss
{
    public class WinlossItemData
    {
        public WinlossItemData()
        {
            Records = new List<RMS_Rpt_Parlay_Winloss>();
            Total = new RMS_Rpt_Parlay_Winloss();
        }

        public List<RMS_Rpt_Parlay_Winloss> Records { get; set; }

        public RMS_Rpt_Parlay_Winloss Total { get; set; }

        public void Add(RMS_Rpt_Parlay_Winloss item)
        {
            if (item != null)
            {
                Records.Add(item);

                Total.Tickets_Won += item.Tickets_Won;
                Total.Tickets_Lose += item.Tickets_Lose;
                Total.Stake_Won += item.Stake_Won;
                Total.Stake_Lose += item.Stake_Lose;
                Total.Member_Won += item.Member_Won;
                Total.Member_Lose += item.Member_Lose;
                Total.OurPT_Won += item.OurPT_Won;
                Total.OurPT_Lose += item.OurPT_Lose;
                Total.OurPT_NetWinloss += item.OurPT_NetWinloss;
                Total.Margin = Total.OurPT_NetWinloss / (Total.Stake_Won + Total.Stake_Lose);
            }
        }
    }
}
