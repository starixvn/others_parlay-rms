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
        /// <param name="username">Username.</param>
        /// <returns>List of RMS_Rpt_Parlay_MemberWinLoss</returns>
        public IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> RMS_Rpt_Parlay_MemberWinloss(DateTime fromDate,
                                                                                      DateTime toDate,
                                                                                      DateTime moveBetsDay,
                                                                                      DateTime balanceUpDay,
                                                                                      int betType,
                                                                                      int sportsType,
                                                                                      string combos,
                                                                                      string username)
        {
            /*
            var result = new List<RMS_Rpt_Parlay_MemberWinLoss>();

            try
            {
                using (var db = new ObjectDb("RMS_Rpt_Parlay_MemberWinLoss"))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                    db.AddInParameter("todate", DbType.DateTime, toDate);
                    db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
                    db.AddInParameter("balanceupday", DbType.DateTime, balanceUpDay);
                    db.AddInParameter("bettype", DbType.Int32, betType);
                    db.AddInParameter("selection", DbType.String, combos);
                    db.AddInParameter("username", DbType.String, username);
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
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
            /**/

            #region Data Sample

            var result = new List<RMS_Rpt_Parlay_MemberWinLoss>();
            result.Add(new RMS_Rpt_Parlay_MemberWinLoss
            {
                Month = "07-2013",
                Tickets_Won = 5512561,
                Tickets_Lose = 2317834,
                Stake_Won = 714980,
                Stake_Lose = 901482,
                Member_Won = -230913,
                Member_Lose = 86145,
                OurPT_Won = -31687,
                OurPT_Lose = 21980
            });
            result.Add(new RMS_Rpt_Parlay_MemberWinLoss
            {
                Month = "08-2013",
                Tickets_Won = 541061,
                Tickets_Lose = 3172834,
                Stake_Won = 611480,
                Stake_Lose = 822781,
                Member_Won = -250732,
                Member_Lose = 411245,
                OurPT_Won = -21487,
                OurPT_Lose = 136000
            });
            result.Add(new RMS_Rpt_Parlay_MemberWinLoss
            {
                Month = "09-2013",
                Tickets_Won = 126571,
                Tickets_Lose = 341864,
                Stake_Won = 112480,
                Stake_Lose = 25182,
                Member_Won = -22313,
                Member_Lose = 47145,
                OurPT_Won = -21751,
                OurPT_Lose = 15090
            });
            return result;

            #endregion Data Sample
        }
    }
}
