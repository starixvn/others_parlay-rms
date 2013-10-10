// --------------------------------------------------------------------------
// <copyright file="WinlossData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Winloss
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.Winloss;

    /// <summary>
    /// Win Loss Data.
    /// </summary>
    public class WinlossData
    {
        public WinlossData()
        {
            this.Records = new List<WinlossItemData>();
            this.Total = new RMS_Rpt_Parlay_Winloss();
        }

        public List<WinlossItemData> Records { get; set; }

        public RMS_Rpt_Parlay_Winloss Total { get; set; }
        
        /// <summary>
        /// Adds new item to Records list and re-calculate to Total.
        /// </summary>
        /// <param name="item">New item needs to add to list.</param>
        public void Add(WinlossItemData item)
        {
            if (item != null)
            {
                this.Records.Add(item);

                // Sum data
                this.Total.Tickets_Won += item.Total.Tickets_Won;
                this.Total.Tickets_Lose += item.Total.Tickets_Lose;
                this.Total.Stake_Won += item.Total.Stake_Won;
                this.Total.Stake_Lose += item.Total.Stake_Lose;
                this.Total.Member_Won += item.Total.Member_Won;
                this.Total.Member_Lose += item.Total.Member_Lose;
                this.Total.OurPT_Won += item.Total.OurPT_Won;
                this.Total.OurPT_Lose += item.Total.OurPT_Lose;
                this.Total.OurPT_NetWinloss += item.Total.OurPT_NetWinloss;
            }
        }
    }
}