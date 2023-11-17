namespace ParlayRMS.DataAccess
{
    using System;
    using System.Data;
    using Fanex.Data;
    using Criteria.SimilarBets;
    using DTO.SimilarBets;
    using Utility;

    public class SimilarBetsDataAccess 
    {
        private const byte runningStatus = 0;
        public RMS_Similar_Parlay_Betlist RMS_Similar_Parlay_Betlist(long transId, bool isSimilar)
        {
            var criteria = new Similar_ComboParlay_Betlist_Criteria
            {
                TransId = transId,
                IsSimilar = isSimilar
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var dataset = db.ExecuteDataSet(criteria))
                {
                    if (dataset.Tables.Count == 2)
                    {
                        var list = dataset.Tables[0];
                        var details = dataset.Tables[1];
                        if (list.Rows.Count > 0)
                        {
                            var info = list.Rows[0];
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
                                IsParlayGenerator = DbConverter.ToBoolean(info["IsParlayGenerator"]),
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
                                    SiteGroup = DbConverter.ToString(row["SiteGroup"]),
                                    Ip = DbConverter.ToString(row["Ip"]),
                                    TransDateString = DbConverter.ToDateTime(info["transdate"]).ToString("MM/dd/yyyy h:mm:ss tt")
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
