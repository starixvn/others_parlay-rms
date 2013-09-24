// --------------------------------------------------------------------------
// <copyright file="MemberWinLossDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.09.16">Created</change>
// </history>

namespace ParlayRMS.DataAccess.Implement.MemberWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.MemberWinLoss;
    using ParlayRMS.DTO.MemberWinLoss;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Member Win Loss Data Access layer.
    /// </summary>
    public class MemberWinLossDataAccess : BaseDataAccess, IMemberWinLossDataAccess
    {
        /// <summary>
        /// Get Member Win Loss data based on passed filter conditions.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="moveBetsDay">The move bets day.</param>
        /// <param name="balanceUpDay">Balance Up Day.</param>
        /// <param name="betType">Bet type ID.</param>
        /// <param name="sportsType">Type of the sports.</param>
        /// <param name="combos">Combos value.</param>
        /// <param name="roleId">Role ID of the customer.</param>
        /// <param name="custid">Customer ID.</param>
        /// <returns>List of RMS_Rpt_Parlay_MemberWinLoss</returns>
        public IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> RMS_Rpt_Parlay_MemberWinloss(DateTime fromDate,
                                                                                      DateTime toDate,
                                                                                      DateTime moveBetsDay,
                                                                                      DateTime balanceUpDay,
                                                                                      int betType,
                                                                                      int sportsType,
                                                                                      string combos,
                                                                                      int roleId,
                                                                                      long custId)
        {
            var result = new List<RMS_Rpt_Parlay_MemberWinLoss>();

            try
            {
                string storeProKey = string.Empty;

                if (roleId == 1)
                {
                    storeProKey = "RMS_Rpt_Parlay_MemberWinLoss";
                }
                else if (roleId == 2)
                {
                    storeProKey = "RMS_Rpt_Parlay_AgentWinLoss";
                }

                if (!string.IsNullOrEmpty(storeProKey))
                {
                    using (var db = new ObjectDb(storeProKey))
                    {
                        db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                        db.AddInParameter("todate", DbType.DateTime, toDate);
                        db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
                        db.AddInParameter("balanceupday", DbType.DateTime, balanceUpDay);
                        db.AddInParameter("selection", DbType.String, combos);
                        db.AddInParameter("custid", DbType.Int32, custId);
                        if (sportsType != 0)
                        {
                            db.AddInParameter("sporttype", DbType.Int32, sportsType);
                        }

                        using (var reader = db.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new RMS_Rpt_Parlay_MemberWinLoss
                                {
                                    Month = reader.ToString("month"),
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
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }
    }
}
