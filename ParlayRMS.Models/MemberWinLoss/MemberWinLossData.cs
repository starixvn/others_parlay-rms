// --------------------------------------------------------------------------
// <copyright file="MemberWinLossData.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.09.16">Created</change>
// </history>

namespace ParlayRMS.Models.MemberWinLoss
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.MemberWinLoss;

    /// <summary>
    /// Member Win Loss Data.
    /// </summary>
    public class MemberWinLossData
    {
        public MemberWinLossData()
        {
            this.Total = new RMS_Rpt_Parlay_MemberWinLoss();
            this.Records = new List<RMS_Rpt_Parlay_MemberWinLoss>();
        }

        public IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> Records { get; set; }

        public RMS_Rpt_Parlay_MemberWinLoss Total { get; set; }
    }
}
