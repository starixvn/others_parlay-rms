// --------------------------------------------------------------------------
// <copyright file="MemberWinLossData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.09.16">Created</change>
// </history>

namespace ParlayRMS.Models.MemberWinLoss
{
    using ParlayRMS.DTO.MemberWinLoss;
    using System.Collections.Generic;

    /// <summary>
    /// Member Win Loss Data.
    /// </summary>
    public class MemberWinLossData
    {
        public IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> Records { get; set; }

        public RMS_Rpt_Parlay_MemberWinLoss Total { get; set; }
    }
}
