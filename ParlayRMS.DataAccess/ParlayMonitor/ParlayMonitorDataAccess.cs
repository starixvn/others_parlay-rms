namespace ParlayRMS.DataAccess.ParlayMonitor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DTO.ParlayMonitor;
    using ParlayRMS.DTO.PotentialWinning;
    using ParlayRMS.Utility;
    using ParlayRMS.Utility.Data;

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
            decimal? odds,
            string leagueGroups,
            long? customerClass,
            string company,
            string username)
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
                db.AddInParameter("iscornerparlay", DbType.Boolean, isCornerParlay);
                db.AddInParameter("odds", DbType.Decimal, odds);
                db.AddInParameter("customerclass", DbType.Int16, customerClass);
                db.AddInParameter("leaguegroupid", DbType.String, leagueGroups);
                db.AddInParameter("company", DbType.String, company);
                if (!string.IsNullOrEmpty(username))
                {
                    db.AddInParameter("UserName", DbType.String, username);
                }  

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Monitor_Parlay_Betlist
                        {
                            TransDate = reader.ToDateTime("transdate"),
                            TransDateString = reader.ToDateTime("transdate").ToString("MM/dd/yyyy h:mm:ss tt"),
                            TransId = reader.ToString("transid"),
                            RefNo = reader.ToString("refno"),
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
                            IsGOF = reader.ToBoolean("IsGOF"),
                            CustomerClass = reader["CustomerClass"] == DBNull.Value ? "None" : reader.ToInt32("CustomerClass").ToString(),
                            TrackingUser = reader["TrackingUser"] == DBNull.Value ? "None" : reader.ToString("TrackingUser"),
                            SiteGroup = reader.ToString("SiteGroup")
                        });
                    }
                }
            }
            return result;
        }

        public UserTracking RMS_Monitor_Parlay_UserTracking(int userid, string username, string transid)
        {
            var userTracking = new UserTracking();
            Hashtable param = new Hashtable();
            param.Add("userid", userid);
            param.Add("username", username);
            param.Add("transid", transid);
            param.Add("statusid", 0);
            param.Add("message", string.Empty);

            var r = ExecSp("RMS_ComboParlay_PotentialWinning_UserTracking", param);
            if (r.IsValid())
            {
                userTracking.StatusId = DbConverter.ToByte(r.Outputs["statusid"]);
                userTracking.Message = DbConverter.ToString(r.Outputs["message"]);
            }

            return userTracking;
        }
    }
}