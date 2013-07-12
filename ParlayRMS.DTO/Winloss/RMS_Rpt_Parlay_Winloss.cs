// -----------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_Winloss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO.Winloss
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RMS_Rpt_Parlay_Winloss
    {
        public DateTime WinlostDate { get; set; }

        public int Tickets_Won { get; set; }

        public int Tickets_Lose { get; set; }

        public decimal Stake_Won { get; set; }

        public decimal Stake_Lose { get; set; }

        public decimal Member_Won { get; set; }

        public decimal Member_Lose { get; set; }

        public decimal OurPT_Won { get; set; }

        public decimal OurPT_Lose { get; set; }
    }
}