using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Fanex.Data;
using ParlayRMS.DTO.SimilarBets;
using ParlayRMS.Utility;

namespace ParlayRMS.DataAccess.Implement.SimilarBets
{
    public class SimilarBetsDataAccess : BaseDataAccess
    {
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        private const byte runningStatus = 0;

        public RMS_Similar_Parlay_Betlist RMS_Similar_Parlay_Betlist(long transId, bool isSimilar)
        {
            using (var db = new ObjectDb("RMS_Similar_Parlay_Betlist"))
            {
                db.AddInParameter("transid", DbType.Int64, transId);
                db.AddInParameter("issimilar", DbType.Boolean, isSimilar);

                using (var dataset = db.ExecuteDataSet())
                {
                    if (dataset.Tables.Count == 2)
                    {
                        DataTable list = dataset.Tables[0];
                        if (list.Rows.Count > 0)
                        {
                            DataRow info = list.Rows[0];
                            DataTable details = dataset.Tables[1];
                            var isRunningStatus = DbConverter.ToByte(info["statusID"]) == runningStatus;

                            return new RMS_Similar_Parlay_Betlist
                            {
                                Transdate = DbConverter.ToDateTime(info["transdate"]),
                                Transid = DbConverter.ToInt64(info["transid"]),
                                Refno = DbConverter.ToInt64(info["refno"]),
                                CustId = DbConverter.ToInt32(info["custid"]),
                                Username = DbConverter.ToString(info["username"]),
                                Odds = DbConverter.ToDecimal(info["odds"]),
                                Stake = DbConverter.ToDecimal(info["stake"]),
                                Max_payout = DbConverter.ToDecimal(info["net_win"]),
                                TotalBet = DbConverter.ToInt32(info["totalbet"]),
                                TotalSelection = Convert.ToInt32(DbConverter.ToDecimal(info["totalselection"])),
                                NumberWon = DbConverter.ToInt32(info["numberwon"]),
                                Level = DbConverter.ToInt32(info["level"]),
                                Cnt_bets = DbConverter.ToInt32(info["cnt_bets"]),
                                SportType = DbConverter.ToInt32(info["sporttype"]),
                                BetType = DbConverter.ToInt32(info["bettype"]),
                                IsGOF = DbConverter.ToBoolean(info["IsGOF"]),
                                StatusID = DbConverter.ToByte(info["statusID"]),
                                PotentialWinningAmount = isRunningStatus ? DbConverter.ToDecimal(info["potentialWinningAmt"]) : 0,
                                HouseLoseAmount = isRunningStatus ? 0 : DbConverter.ToDecimal(info["houseLoseAmt"]),
                                CustomerClass = info["CustomerClass"] == DBNull.Value ? "None" : DbConverter.ToString(info["CustomerClass"]),
                                SiteGroup = DbConverter.ToString(info["SiteGroup"]),
                                Details = details.AsEnumerable().Select(row => new RMS_Similar_Parlay_Betlist_Detail
                                {
                                    TransId = DbConverter.ToInt64(row["transid"]),
                                    Refno = DbConverter.ToInt64(row["refno"]),
                                    Username = DbConverter.ToString(row["username"]),
                                    CustId = DbConverter.ToInt32(row["custid"]),
                                    Level = DbConverter.ToInt32(row["level"]),
                                    Stake = DbConverter.ToDecimal(row["stake"]),
                                    Max_Payout = DbConverter.ToDecimal(row["net_win"]),
                                    PotentialWinningAmount = isRunningStatus ? DbConverter.ToDecimal(row["potentialWinningAmt"]) : 0,
                                    HouseLoseAmount = isRunningStatus ? 0 : DbConverter.ToDecimal(row["houseLoseAmt"]),
                                    Similar_Selection = DbConverter.ToInt32(row["similar_selection"]),
                                    CustomerClass = row["CustomerClass"] == DBNull.Value ? "None" : DbConverter.ToString(row["CustomerClass"]),
                                    SiteGroup = DbConverter.ToString(info["SiteGroup"])
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