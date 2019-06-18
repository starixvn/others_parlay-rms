using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using ParlayRMS.DTO.MemberWinLoss;
using ParlayRMS.Utility.Data;

namespace ParlayRMS.DataAccess.Implement.MemberWinLoss
{
    public class MemberWinLossDataAccess : BaseDataAccess
    {
        public IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> RMS_Rpt_Parlay_MemberWinLoss(
                                                             DateTime fromDate,
                                                             DateTime toDate,
                                                             DateTime moveBetsDay,
                                                             DateTime balanceUpDay,
                                                             string parlayTypes,
                                                             string sportsType,
                                                             string combos,
                                                             long custId)
        {
            var result = new List<RMS_Rpt_Parlay_MemberWinLoss>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_MemberWinLoss"))
            {
                db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                db.AddInParameter("todate", DbType.DateTime, toDate);
                db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
                db.AddInParameter("balanceupday", DbType.DateTime, balanceUpDay);
                db.AddInParameter("selection", DbType.String, combos);
                db.AddInParameter("parlaytype", DbType.String, parlayTypes);
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("sporttype", DbType.String, sportsType);

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

            return result;
        }

        public IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> RMS_Rpt_Parlay_AgentWinLoss(
                                                             DateTime fromDate,
                                                             DateTime toDate,
                                                             DateTime moveBetsDay,
                                                             DateTime balanceUpDay,
                                                             string parlayTypes,
                                                             string sportsType,
                                                             string combos,
                                                             long custId)
        {
            var result = new List<RMS_Rpt_Parlay_MemberWinLoss>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_AgentWinLoss"))
            {
                db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                db.AddInParameter("todate", DbType.DateTime, toDate);
                db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
                db.AddInParameter("balanceupday", DbType.DateTime, balanceUpDay);
                db.AddInParameter("parlaytype", DbType.String, parlayTypes);
                db.AddInParameter("selection", DbType.String, combos);
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("sporttype", DbType.String, sportsType);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_MemberWinLoss
                        {
                            Month = reader.ToString("month"),
                            Username = reader.ToString("username"),
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