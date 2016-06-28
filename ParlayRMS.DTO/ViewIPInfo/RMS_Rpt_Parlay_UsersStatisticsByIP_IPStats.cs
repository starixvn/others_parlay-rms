// --------------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_UsersStatisticsByIP_IPStats.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.6.30">Created</change>
// </history>

namespace ParlayRMS.DTO.ViewIPInfo
{
    using System;

    /// <summary>
    /// Users statistics by IP - IP rows model.
    /// </summary>
    public class RMS_Rpt_Parlay_UsersStatisticsByIP_IPStats
    {
        public string IP { get; set; }

        public int? CntUsers { get; set; }
    }
}