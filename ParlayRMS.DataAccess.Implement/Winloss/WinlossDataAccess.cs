// --------------------------------------------------------------------------
// <copyright file="WinlossDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement.Winloss
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.Winloss;
    using ParlayRMS.DTO.Winloss;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Win Loss Data Access.
    /// </summary>
    public class WinlossDataAccess : BaseDataAccess, IWinlossDataAccess
    {
        /// <summary>
        /// Gets Win Loss data based on passed filter conditions.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="moveBetsDay">The move bets day.</param>
        /// <param name="balanceUpDay">Balance Up Day.</param>
        /// <param name="betType">Bet type ID.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>List of Win Loss objects.</returns>
        public IEnumerable<RMS_Rpt_Parlay_Winloss> RMS_Rpt_Parlay_Winloss(
                                                       DateTime fromDate, 
                                                       DateTime toDate, 
                                                       DateTime moveBetsDay, 
                                                       DateTime balanceUpDay, 
                                                       int betType, 
                                                       int sportsType, 
                                                       string combos)
        {
            var result = new List<RMS_Rpt_Parlay_Winloss>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_Winloss"))
            {
                db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                db.AddInParameter("todate", DbType.DateTime, toDate);
                db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
                db.AddInParameter("balanceupday", DbType.DateTime, balanceUpDay);
                db.AddInParameter("bettype", DbType.Int32, betType);
                db.AddInParameter("selection", DbType.String, combos);
                if (sportsType != 0)
                {
                    db.AddInParameter("sporttype", DbType.Int32, sportsType);
                }

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_Winloss
                        {
                            Combo = reader.ToInt32("combo").ToString(),
                            WinlostDate = reader.ToDateTime("winlostdate"),
                            Tickets_Won = reader.ToInt32("tickets_won"),
                            Tickets_Lose = reader.ToInt32("tickets_lose"),
                            Stake_Won = reader.ToDecimal("stake_won"),
                            Stake_Lose = reader.ToDecimal("stake_lose"),
                            Member_Won = reader.ToDecimal("member_won"),
                            Member_Lose = reader.ToDecimal("member_lose"),
                            OurPT_Won = reader.ToDecimal("ourpt_won"),
                            OurPT_Lose = reader.ToDecimal("ourpt_lose")
                        });
                    }
                }
            }

            return result;
        }
    }
}