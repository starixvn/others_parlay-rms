using ParlayRMS.DTO.Winloss;
using System.Collections.Generic;

namespace ParlayRMS.Models.Winloss
{
    public class WinlossData
    {
        public WinlossData()
        {
            Records = new List<WinlossItemData>();
            Total = new RMS_Rpt_Parlay_Winloss();
        }

        public List<WinlossItemData> Records { get; set; }

        public RMS_Rpt_Parlay_Winloss Total { get; set; }

        public void Add(WinlossItemData item)
        {
            if (item != null)
            {
                Records.Add(item);

                // Sum data
                Total.Tickets_Won += item.Total.Tickets_Won;
                Total.Tickets_Lose += item.Total.Tickets_Lose;
                Total.Stake_Won += item.Total.Stake_Won;
                Total.Stake_Lose += item.Total.Stake_Lose;
                Total.Member_Won += item.Total.Member_Won;
                Total.Member_Lose += item.Total.Member_Lose;
                Total.OurPT_Won += item.Total.OurPT_Won;
                Total.OurPT_Lose += item.Total.OurPT_Lose;
                Total.OurPT_NetWinloss += item.Total.OurPT_NetWinloss;
            }
        }
    }
}