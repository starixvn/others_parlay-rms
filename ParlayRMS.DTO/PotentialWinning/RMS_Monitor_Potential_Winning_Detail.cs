// --------------------------------------------------------------------------
// <copyright file="RMS_Monitor_Potential_Winning_Detail.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.PotentialWinning
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// Potential Winning Detail model.
    /// </summary>
    public class RMS_Monitor_Potential_Winning_Detail
    {
        public DateTime TransDate { get; set; }

        public string TransDateString { get; set; }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public decimal Odds { get; set; }

        public decimal AverageStake { get; set; }

        public decimal Stake { get; set; }

        public int SportType { get; set; }

        public decimal WinningAmount { get; set; }

        public decimal CompanyLoseAmount { get; set; }

        public string Ip { get; set; }

        public decimal HousePT { get; set; }

        public int TotalBet { get; set; }

        public int TotalSelection { get; set; }

        public List<string> ComboTypes { get; set; } = new List<string> { "Double (3)", "Trebles (1)" };

        public int Combo { get; set; }
    }
}