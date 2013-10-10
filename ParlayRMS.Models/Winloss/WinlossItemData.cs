// --------------------------------------------------------------------------
// <copyright file="WinlossItemData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.08">Separated from WinlossData</change>
// </history>

namespace ParlayRMS.Models.Winloss
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.Winloss;

    /// <summary>
    /// Win Loss Item Data.
    /// </summary>
    public class WinlossItemData
    {
        public WinlossItemData()
        {
            this.Records = new List<RMS_Rpt_Parlay_Winloss>();
            this.Total = new RMS_Rpt_Parlay_Winloss();
        }

        public List<RMS_Rpt_Parlay_Winloss> Records { get; set; }

        public RMS_Rpt_Parlay_Winloss Total { get; set; }

        /// <summary>
        /// Adds new item to Records list and re-calculate to Total.
        /// </summary>
        /// <param name="item">New item needs to add to list.</param>
        public void Add(RMS_Rpt_Parlay_Winloss item)
        {
            if (item != null)
            {
                this.Records.Add(item);

                // Sum data
                this.Total.Tickets_Won += item.Tickets_Won;
                this.Total.Tickets_Lose += item.Tickets_Lose;
                this.Total.Stake_Won += item.Stake_Won;
                this.Total.Stake_Lose += item.Stake_Lose;
                this.Total.Member_Won += item.Member_Won;
                this.Total.Member_Lose += item.Member_Lose;
                this.Total.OurPT_Won += item.OurPT_Won;
                this.Total.OurPT_Lose += item.OurPT_Lose;
                this.Total.OurPT_NetWinloss += item.OurPT_NetWinloss;
            }
        }
    }
}