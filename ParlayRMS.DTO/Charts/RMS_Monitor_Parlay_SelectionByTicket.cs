// --------------------------------------------------------------------------
// <copyright file="RMS_Monitor_Parlay_SelectionByTicket.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Charts
{
    /// <summary>
    /// RMS_Monitor_Parlay_SelectionByTicket model.
    /// </summary>
    public class RMS_Monitor_Parlay_SelectionByTicket
    {
        public int All_Sports { get; set; }

        public int Combined_Sports { get; set; }

        public int Other_Sports { get; set; }

        public int Selection { get; set; }

        public int Soccer_Only { get; set; }
    }
}