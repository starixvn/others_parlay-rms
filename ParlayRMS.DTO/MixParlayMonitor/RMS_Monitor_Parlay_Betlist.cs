// -----------------------------------------------------------------------
// <copyright file="RMS_Monitor_Parlay_Betlist.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO.MixParlayMonitor
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RMS_Monitor_Parlay_Betlist
    {
        public DateTime TransDate { get; set; }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public string Username { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal MaxPayout { get; set; }

        public int Combo { get; set; }

        public int Level { get; set; }
    }
}