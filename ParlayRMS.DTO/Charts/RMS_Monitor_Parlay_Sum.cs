// --------------------------------------------------------------------------
// <copyright file="RMS_Monitor_Parlay_Sum.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Charts
{
    /// <summary>
    /// RMS_Monitor_Parlay_Sum model.
    /// </summary>
    public class RMS_Monitor_Parlay_Sum
    {
        public int Events { get; set; }

        public string Status { get; set; }

        public int Tickets { get; set; }

        public decimal Turnover { get; set; }

        public decimal Winloss { get; set; }

        public decimal CompanyWL { get; set; }
    }
}