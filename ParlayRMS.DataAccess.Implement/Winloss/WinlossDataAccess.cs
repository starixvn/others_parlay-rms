using Fanex.Data;
using ParlayRMS.DataAccess.Interface.Winloss;
using ParlayRMS.DTO.Winloss;
using ParlayRMS.Utility.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace ParlayRMS.DataAccess.Implement.Winloss
{
    public class WinlossDataAccess : BaseDataAccess, IWinlossDataAccess
    {
        public IEnumerable<RMS_Rpt_Parlay_Winloss> RMS_Rpt_Parlay_Winloss(
                                                       DateTime fromDate,
                                                       DateTime toDate,
                                                       DateTime moveBetsDay,
                                                       DateTime balanceUpDay,
                                                       string parlayTypes,
                                                       int sportsType,
                                                       string combos)
        {
            var result = new List<RMS_Rpt_Parlay_Winloss>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_Winloss"))
            {
                db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                db.AddInParameter("todate", DbType.DateTime, toDate);
                db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
                db.AddInParameter("balanceupday", DbType.DateTime, balanceUpDay);
                db.AddInParameter("parlaytype", DbType.String, parlayTypes);
                db.AddInParameter("selection", DbType.String, combos);
                if (sportsType != 0)
                {
                    db.AddInParameter("sporttype", DbType.Int32, sportsType);
                }

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_Winloss
                        {
                            Combo = reader.ToInt32("combo").ToString(),
                            WinlostDate = reader.ToDateTime("winlostdate"),
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