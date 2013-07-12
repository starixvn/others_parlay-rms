// -----------------------------------------------------------------------
// <copyright file="WinlossDetail.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Models.Winloss
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.Winloss;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinlossData
    {
        public IEnumerable<RMS_Rpt_Parlay_Winloss> Records { get; set; }

        public RMS_Rpt_Parlay_Winloss Total { get; set; }
    }
}