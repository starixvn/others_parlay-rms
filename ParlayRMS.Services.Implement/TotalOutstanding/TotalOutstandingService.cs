// --------------------------------------------------------------------------
// <copyright file="TotalOutstandingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.TotalOutstanding
{
    using ParlayRMS.DataAccess.Implement.TotalOutstanding;
    using ParlayRMS.DataAccess.Interface.TotalOutstanding;
    using ParlayRMS.DTO.TotalOutstanding;
    using ParlayRMS.Services.Interface.TotalOutstanding;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Total Outstanding Service.
    /// </summary>
    public class TotalOutstandingService : ITotalOutstandingService
    {
        private readonly ITotalOutstandingDataAccess _dataAccess = new TotalOutstandingDataAccess();

        /// <summary>
        /// Gets parlay outstanding of a member.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="combo">Combo values.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <returns>Parlay Outstanding of a member.</returns>
        public List<RMS_Rpt_Parlay_Outstanding_Mem> GetTotalOutstandingForMember(long custId, string combo, string sportsType, string parlaysType)
        {
            var data = _dataAccess.RMS_Rpt_Parlay_Outstanding_Mem(custId, combo, sportsType, parlaysType);
            foreach (var item in data)
            {
                item.StakePt = Math.Round(item.StakePt, 2);
                item.MaxPayoutPt = Math.Floor(item.MaxPayoutPt);
            }

            return data;
        }

        /// <summary>
        /// Gets parlay outstanding of a agent.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="combo">Combo values.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <returns>Parlay Outstanding of a agent.</returns>
        public List<RMS_Rpt_Parlay_Outstanding_Ag> GetTotalOutstandingForAgent(long custId, string combo, string sportsType, string parlaysType)
        {
            var data = _dataAccess.RMS_Rpt_Parlay_Outstanding_Ag(custId, combo, sportsType, parlaysType);
            foreach (var item in data)
            {
                item.StakePt = Math.Round(item.StakePt, 2);
                item.MaxPayoutPt = Math.Floor(item.MaxPayoutPt);
            }

            return data;
        }
    }
}