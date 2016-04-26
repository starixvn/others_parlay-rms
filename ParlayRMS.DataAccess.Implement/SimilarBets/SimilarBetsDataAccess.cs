using Fanex.Data;
using ParlayRMS.DataAccess.Interface.SimilarBets;
using ParlayRMS.DTO.SimilarBets;
using ParlayRMS.Utility;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace ParlayRMS.DataAccess.Implement.SimilarBets
{
    public class SimilarBetsDataAccess : BaseDataAccess, ISimilarBetsDataAccess
    {
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public RMS_Similar_Parlay_Betlist RMS_Similar_Parlay_Betlist(long transId)
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
                                Max_payout = DbConverter.ToDecimal(info["net_win"]),
                                TotalBet = DbConverter.ToInt32(info["totalbet"]),
                                TotalSelection = Convert.ToInt32(DbConverter.ToDecimal(info["totalselection"])),
                                Level = DbConverter.ToInt32(info["level"]),
                                Cnt_bets = DbConverter.ToInt32(info["cnt_bets"]),
                                Details = details.AsEnumerable().Select(row => new RMS_Similar_Parlay_Betlist_Detail
                                {
                                    TransId = DbConverter.ToInt64(row["transid"]),
                                    Refno = DbConverter.ToInt64(row["refno"]),
                                    Username = DbConverter.ToString(row["username"]),
                                    Level = DbConverter.ToInt32(row["level"]),
                                    Max_Payout = DbConverter.ToDecimal(row["net_win"]),
                                    Similar_Selection = DbConverter.ToInt32(row["similar_selection"])
                                })
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}