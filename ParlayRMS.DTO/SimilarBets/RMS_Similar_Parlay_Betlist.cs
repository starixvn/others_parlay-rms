// --------------------------------------------------------------------------
// <copyright file="RMS_Similar_Parlay_Betlist.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.SimilarBets
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Similar Bet List model.
    /// </summary>
    public class RMS_Similar_Parlay_Betlist
    {
        public int Cnt_bets { get; set; }

        public int Combo { get; set; }

        public int Level { get; set; }

        public decimal Max_payout { get; set; }

        public decimal Odds { get; set; }

        public long Refno { get; set; }

        public decimal Stake { get; set; }

        public DateTime Transdate { get; set; }

        public long Transid { get; set; }

        public string Username { get; set; }

        public IEnumerable<RMS_Similar_Parlay_Betlist_Detail> Details { get; set; }
    }
}