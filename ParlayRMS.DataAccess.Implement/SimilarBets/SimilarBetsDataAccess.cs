// --------------------------------------------------------------------------
// <copyright file="SimilarBetsDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement.SimilarBets
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.SimilarBets;
    using ParlayRMS.DTO.SimilarBets;
    using ParlayRMS.Utility;

    /// <summary>
    /// Similar Bets Data Access.
    /// </summary>
    public class SimilarBetsDataAccess : BaseDataAccess, ISimilarBetsDataAccess
    {
        /// <summary>
        /// Gets similar bet list.
        /// </summary>
        /// <param name="transId">Transaction ID.</param>
        /// <returns>List of similar bets.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public RMS_Similar_Parlay_Betlist RMS_Similar_Parlay_Betlist(long transId)
        {
            try
            {
                using (var db = new ObjectDb("RMS_Similar_Parlay_Betlist"))
                {
                    db.AddInParameter("transid", DbType.Int64, transId);

                    using (var dataset = db.ExecuteDataSet())
                    {
                        if (dataset.Tables.Count == 2)
                        {
                            DataTable list = dataset.Tables[0];
                            if (list.Rows.Count > 0)
                            {
                                DataRow info = list.Rows[0];
                                DataTable details = dataset.Tables[1];

                                return new RMS_Similar_Parlay_Betlist
                                {
                                    Transdate = DbConverter.ToDateTime(info["transdate"]),
                                    Transid = DbConverter.ToInt64(info["transid"]),
                                    Refno = DbConverter.ToInt64(info["refno"]),
                                    Username = DbConverter.ToString(info["username"]),
                                    Odds = DbConverter.ToDecimal(info["odds"]),
                                    Stake = DbConverter.ToDecimal(info["stake"]),
                                    Max_payout = DbConverter.ToDecimal(info["max_payout"]),
                                    Combo = DbConverter.ToInt32(info["combo"]),
                                    Level = DbConverter.ToInt32(info["level"]),
                                    Cnt_bets = DbConverter.ToInt32(info["cnt_bets"]),
                                    Details = details.AsEnumerable().Select(row => new RMS_Similar_Parlay_Betlist_Detail
                                    {
                                        TransId = DbConverter.ToInt64(row["transid"]),
                                        Refno = DbConverter.ToInt64(row["refno"]),
                                        Username = DbConverter.ToString(row["username"]),
                                        Level = DbConverter.ToInt32(row["level"]),
                                        Max_Payout = DbConverter.ToDecimal(row["max_payout"]),
                                        Similar_Combo = DbConverter.ToInt32(row["similar_combo"])
                                    })
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return null;
        }
    }
}