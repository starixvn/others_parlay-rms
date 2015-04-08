// --------------------------------------------------------------------------
// <copyright file="BetListDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.BetList;
    using ParlayRMS.DTO.BetList;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Bet List Data Access.
    /// </summary>
    public class BetListDataAccess : BaseDataAccess, IBetListDataAccess
    {
        /// <summary>
        /// Gets transaction details of a bet.
        /// </summary>
        /// <param name="refNo">Reference number.</param>
        /// <returns>List of transactions.</returns>
        public IEnumerable<RMS_Parlay_TransDetails> RMS_Parlay_TransDetails(long refNo)
        {
            var result = new List<RMS_Parlay_TransDetails>();

            using (var db = new ObjectDb("RMS_Parlay_TransDetails"))
            {
                db.AddInParameter("refno", DbType.Int64, refNo);
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Parlay_TransDetails
                        {
                            TransId = reader.ToInt64("transid"),
                            BetType = reader.ToInt32("bettype"),
                            HomeId = reader.ToInt32("homeid"),
                            AwayId = reader.ToInt32("awayid"),
                            Hdp1 = reader.ToDecimal("hdp1"),
                            Hdp2 = reader.ToDecimal("hdp2"),
                            BetTeam = reader.ToString("betteam"),
                            Odds = reader.ToDecimal("odds"),
                            StatusId = reader.ToInt32("statusid"),
                            Status = reader.ToString("status"),
                            LiveHomeScore = reader.ToInt32("livehomescore"),
                            LiveAwayScore = reader.ToInt32("liveawayscore"),
                            LiveIndicator = reader.ToBoolean("liveindicator"),
                            LeagueId = reader.ToInt32("leagueid"),
                            SportType = reader.ToInt32("sporttype"),
                            IsNeutral = reader.ToBoolean("IsNeutral"),
                            MatchId = reader.ToInt32("matchid"),
                            MatchCode = reader.ToInt32("matchcode"),
                            BetStatus = reader.ToString("betstatus"),
                            BetCheck = reader.ToString("betcheck")
                        });
                    }
                }
            }

            return result;
        }
    }
}