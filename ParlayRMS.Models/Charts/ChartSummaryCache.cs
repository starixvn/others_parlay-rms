// --------------------------------------------------------------------------
// <copyright file="ChartSummaryCache.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Charts
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.Charts;

    /// <summary>
    /// Chart Summary Cache.
    /// </summary>
    public class ChartSummaryCache
    {
        public IEnumerable<RMS_Monitor_Parlay_Sum> Sum
        { 
            get; 
            set; 
        }
                
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket> SelectionByTicket
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover> SelectionByTurnover
        { 
            get; 
            set; 
        }
    }
}