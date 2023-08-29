namespace ParlayRMS.DataAccess.PotentialWinningMonitor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using DTO.PotentialWinning;
    using Utility;
    using Utility.Data;

    public class PotentialWinningMonitorDataAccess : BaseDataAccess
    {
        public IEnumerable<RMS_Monitor_Potential_Winning> RMS_Monitor_Parlay_PotentialWinning(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal winningAmount,
            int selectTop,
            bool isCornerParlay,
            byte filterBy,
            int? agentId,
            long? customerClass,
            string company,
            string username)
        {
            var result = new List<RMS_Monitor_Potential_Winning>();

            using (var db = new ObjectDb("RMS_Monitor_Parlay_PotentialWinning"))
            {
                db.AddInParameter("selection", DbType.String, combos);
                db.AddInParameter("parlaytype", DbType.String, parlayTypes);
                db.AddInParameter("sporttype", DbType.String, sportsTypes);
                db.AddInParameter("level", DbType.String, levels);
                db.AddInParameter("winningamount", DbType.Decimal, winningAmount);
                db.AddInParameter("limitRowCount", DbType.Int32, selectTop);
                db.AddInParameter("iscornerparlay", DbType.Boolean, isCornerParlay);
                db.AddInParameter("isagentview", DbType.Boolean, filterBy > 0);
                db.AddInParameter("recommend", DbType.Int32, agentId);
                db.AddInParameter("customerclass", DbType.Int16, customerClass);
                db.AddInParameter("company", DbType.String, company);
                if (!string.IsNullOrEmpty(username))
                {
                    db.AddInParameter("UserName", DbType.String, username);
                }

                using (var reader = db.ExecuteReader())
                {
                    if (filterBy > 0)
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Potential_Winning
                            {
                                AgentName = reader.ToString("agentname"),
                                WinningAmount = reader.ToDecimal("winningamount"),
                                TotalMember = reader.ToInt32("totalmember"),
                                Recommend = reader.ToInt32("recommend"),
                                Stake = reader.ToDecimal("TotalStake")
                            });
                        }
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Potential_Winning
                            {
                                TransDate = reader.ToDateTime("transdate"),
                                TransDateString = reader.ToDateTime("transdate").ToString("MM/dd/yyyy h:mm:ss tt"),
                                CustId = reader.ToInt64("custid"),
                                UserName = reader.ToString("username"),
                                Currency = reader.ToString("currency"),
                                Odds = reader.ToDecimal("odds"),
                                Stake = reader.ToDecimal("stake"),
                                WinningAmount = reader.ToDecimal("winningamount"),
                                CompanyLoseAmount = reader.ToDecimal("companyloseamount"),
                                Level = reader.ToInt32("level"),
                                TotalTicket = reader.ToInt32("totalbets"),
                                CustomerClass = reader["CustomerClass"] == DBNull.Value ? "None" : reader.ToInt32("CustomerClass").ToString(),
                                TotalTrackingBets = reader.ToInt32("TotalTrackingBets"),
                                SiteGroup = reader.ToString("SiteGroup")
                            });
                        }
                    }
                }
            }

            return result;
        }

        public IEnumerable<RMS_Monitor_Potential_Winning_Detail> RMS_Monitor_Parlay_PotentialWinningDetail(
            string combos,
            string parlayTypes,
            string sportsTypes,
            long custId,
            bool isCornerParlay)
        {
            var result = new List<RMS_Monitor_Potential_Winning_Detail>();

            using (var db = new ObjectDb("RMS_Monitor_Parlay_PotentialWinningDetails"))
            {
                db.AddInParameter("custid", DbType.Int64, custId);
                db.AddInParameter("selection", DbType.String, combos);
                db.AddInParameter("parlaytype", DbType.String, parlayTypes);
                db.AddInParameter("sporttype", DbType.String, sportsTypes);
                db.AddInParameter("IsCornerParlay", DbType.Boolean, isCornerParlay);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Monitor_Potential_Winning_Detail
                        {
                            TransDate = reader.ToDateTime("transdate"),
                            TransDateString = reader.ToDateTime("transdate").ToString("MM/dd/yyyy h:mm:ss tt"),
                            TransId = reader.ToInt64("transid"),
                            Odds = reader.ToDecimal("odds"),
                            RefNo = reader.ToInt64("refno"),
                            Stake = reader.ToDecimal("stake"),
                            WinningAmount = reader.ToDecimal("winningamount"),
                            CompanyLoseAmount = reader.ToDecimal("companyloseamount"),
                            Ip = reader.ToString("ip"),
                            HousePT = reader.ToDecimal("housept"),
                            TotalBet = reader.ToInt32("totalbet"),
                            TotalSelection = Convert.ToInt32(reader.ToDecimal("totalselection")),
                            NumberWon = Convert.ToInt32(reader.ToDecimal("numberwon")),
                            SportType = reader.ToInt32("sporttype"),
                            BetType = reader.ToInt32("bettype"),
                            IsGOF = reader.ToBoolean("IsGOF"),
                            TrackingUser = reader["TrackingUser"] == DBNull.Value ? "None" : reader.ToString("TrackingUser")
                        });
                    }
                }
            }

            return result;
        }

        public UserTracking RMS_ComboParlay_PotentialWinning_UserTracking(int userid, string username, string transid)
        {
            var userTracking = new UserTracking();
            var param = new Hashtable
            {
                { "userid", userid },
                { "username", username },
                { "transid", transid },
                { "statusid", 0 },
                { "message", string.Empty }
            };

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
