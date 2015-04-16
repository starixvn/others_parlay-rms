// --------------------------------------------------------------------------
// <copyright file="PotentialWinningDetailModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.Monitoring
{
    /// <summary>
    /// Data Model.
    /// </summary>
    public class PotentialWinningDetailModel
    {
        public string Combos { get; set; }

        public string SportsTypes { get; set; }
        
        public long CustId { get; set; }

        public string UserName { get; set; }
    }
}