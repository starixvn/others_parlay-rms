// --------------------------------------------------------------------------
// <copyright file="TotalDataModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.TotalOutstanding
{
    /// <summary>
    /// Total Data Model.
    /// </summary>
    public class TotalDataModel
    {
        public decimal StakePt { get; set; }

        public decimal MaxPayoutPt { get; set; }

        public int TotalBets { get; set; }
    }
}