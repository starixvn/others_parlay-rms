// --------------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_UsersStatisticsByIP.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.6.30">Created</change>
// </history>

namespace ParlayRMS.DTO.ViewIPInfo
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Users statistics by IP model.
    /// </summary>
    public class RMS_Rpt_Parlay_UsersStatisticsByIP
    {
        public int TotalItems { get; set; }

        public long MaxTransId { get; set; }

        public IEnumerable<RMS_Rpt_Parlay_UsersStatisticsByIP_IPStats> IPStats { get; set; }
    }
}