// --------------------------------------------------------------------------
// <copyright file="RMS_Monitor_Parlay_SelectionByTurnover.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Charts
{
    /// <summary>
    /// RMS_Monitor_Parlay_SelectionByTurnover model.
    /// </summary>
    public class RMS_Monitor_Parlay_SelectionByTurnover
    {
        public decimal All_Sports { get; set; }

        public decimal Combined_Sports { get; set; }

        public decimal Other_Sports { get; set; }

        public int Selection { get; set; }

        public decimal Soccer_Only { get; set; }
    }
}