// --------------------------------------------------------------------------
// <copyright file="RMS_SystemParlay_CombinationDetails_Combo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.24">Created</change>
// </history>

namespace ParlayRMS.DTO.ViewIPInfo
{
    using System;

    /// <summary>
    /// CombinationDetails Combo model.
    /// </summary>
    public class RMS_SystemParlay_CombinationDetails_Combo
    {
        public DateTime WinLostDate { get; set; }

        public long TransID { get; set; }

        public int Serial { get; set; }

        public int StatusId { get; set; }

        public String Status { get; set; }

        public double Modds { get; set; }

        public double Stake { get; set; }

        public double Winlost { get; set; }
    }
}