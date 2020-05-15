using Fanex.Data;
using ParlayRMS.DTO.ParlayWon;
using ParlayRMS.Utility.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace ParlayRMS.DataAccess.Implement.ParlayWon
{
    public class ParlayWonDataAccess : BaseDataAccess
    {
        public IEnumerable<RMS_Rpt_Parlay_ParlayWon> RMS_Rpt_Parlay_ParlayWon(
                                                       DateTime fromDate,
                                                       DateTime toDate,
                                                       DateTime moveBetsDay,
                                                       string parlayTypes,
                                                       string sportsType,
                                                       string combos,
                                                       string level,
                                                       decimal houseAmountFrom,
                                                       decimal houseAmountTo)
        {
            var result = new List<RMS_Rpt_Parlay_ParlayWon>();

            using (var db = new ObjectDb("RMS_Rpt_ParlayWon"))
            {
                db.AddInParameter("FromDate", DbType.DateTime, fromDate);
                db.AddInParameter("ToDate", DbType.DateTime, toDate);
                db.AddInParameter("MoveBetsDay", DbType.DateTime, moveBetsDay);
                db.AddInParameter("ParlayType", DbType.String, parlayTypes);
                db.AddInParameter("Selection", DbType.String, combos);
                db.AddInParameter("Sporttype", DbType.String, sportsType);
                db.AddInParameter("Level", DbType.String, level);
                db.AddInParameter("FromHouseAmt", DbType.Decimal, houseAmountFrom);
                db.AddInParameter("ToHouseAmt", DbType.Decimal, houseAmountTo);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_ParlayWon
                        {
                            TransDate = reader.ToDateTime("transdate"),
                            TransId = reader.ToString("transid"),
                            RefNo = reader.ToString("refno"),
                            CustId = reader.ToInt32("custid"),
                            Username = reader.ToString("username"),
                            BetType = reader.ToInt32("bettype"),
                            Odds = reader.ToDecimal("odds"),
                            Stake = reader.ToDecimal("Stake"),
                            StakeAfterPT = reader.ToDecimal("StakeAfterPT"),
                            WinningAmount = reader.ToDecimal("WinningAmount"),
                            HouseLoseAmount = reader.ToDecimal("HouseLoseAmount"),
                            TotalBet = reader.ToInt32("TotalBet"),
                            TotalSelection = reader.ToInt32("TotalSelection"),
                            SportType = reader.ToInt32("SportType"),
                            IP = reader.ToString("IP")
                        });
                    }
                }
            }

            return result;
        }
    }
}
