using Fanex.Data;
using ParlayRMS.DTO.ParlayMonitor;
using ParlayRMS.Utility.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace ParlayRMS.DataAccess.Implement.ParlayMonitor
{
    public class ParlayMonitorDataAccess : BaseDataAccess
    {
        public IEnumerable<RMS_Monitor_Parlay_Betlist> RMS_Monitor_Parlay_Betlist(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal stakeAfterPT,
            decimal maxPayout,
            int reportType,
            int selectTop,
            long maxTransId,
            string markets,
            bool isCornerParlay,
            decimal? odds)
        {
            var result = new List<RMS_Monitor_Parlay_Betlist>();

            using (var db = new ObjectDb("RMS_Monitor_Parlay_Betlist"))
            {
                db.AddInParameter("selection", DbType.String, combos);
                db.AddInParameter("parlaytype", DbType.String, parlayTypes);
                db.AddInParameter("sporttype", DbType.String, sportsTypes);
                db.AddInParameter("level", DbType.String, levels);
                db.AddInParameter("stake", DbType.Decimal, stakeAfterPT);
                db.AddInParameter("maxpayout", DbType.Decimal, maxPayout);
                db.AddInParameter("reporttype", DbType.Int32, reportType);
                db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                db.AddInParameter("limitrowcount", DbType.Int32, selectTop);
                db.AddInParameter("market", DbType.String, markets);
                db.AddInParameter("IsCornerParlay", DbType.Boolean, isCornerParlay);
                db.AddInParameter("odds", DbType.Decimal, odds);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Monitor_Parlay_Betlist
                        {
                            TransDate = reader.ToDateTime("transdate"),
                            TransDateString = reader.ToDateTime("transdate").ToString("MM/dd/yyyy h:mm:ss tt"),
                            TransId = reader.ToInt64("transid"),
                            RefNo = reader.ToInt64("refno"),
                            Username = reader.ToString("username"),
                            Odds = reader.ToDecimal("odds"),
                            Stake = reader.ToDecimal("stake"),
                            PotentialWinningAmount = reader.ToDecimal("potentialWinningAmt"),
                            NetWinAfterPT = reader.ToDecimal("maxpayout"),
                            TotalSelection = Convert.ToInt32(reader.ToDecimal("totalselection")),
                            TotalBet = reader.ToInt32("totalbet"),
                            NumberWon = reader.ToInt32("numberwon"),
                            IP = reader.ToString("ip"),
                            Level = reader.ToInt32("level"),
                            IsBetRadar = reader.ToBoolean("IsBetRadar"),
                            SportType = reader.ToInt32("sporttype"),
                            BetType = reader.ToInt32("bettype"),
                            CustId = reader.ToInt32("custid"),
                            IsLive = reader.ToBoolean("islive"),
                            IsBlacklistLeague = Convert.ToInt32(reader["isblacklistleague"]) == 1,
                            IsGOF = reader.ToBoolean("IsGOF")
                        });
                    }
                }
            }

            return result;
        }
    }
}