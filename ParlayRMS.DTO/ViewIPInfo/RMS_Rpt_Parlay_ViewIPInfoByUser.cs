// --------------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_ViewIPInfoByUser.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.22">Created</change>
// </history>

namespace ParlayRMS.DTO.ViewIPInfo
{
    using System;

    /// <summary>
    /// View IP Info by user model, use for drill down report.
    /// </summary>
    public class RMS_Rpt_Parlay_ViewIPInfoByUser
    {
        public long TransID { get; set; }

        public long RefNo { get; set; }

        public DateTime TransDate { get; set; }

        public int BetType { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public decimal Winlost { get; set; }

        public int Hdp1 { get; set; }

        public int Hdp2 { get; set; }

        public string BetTeam { get; set; }

        public string BetCheck { get; set; }
    }
}