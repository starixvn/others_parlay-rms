using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using ParlayRMS.DTO.TotalOutstanding;
using ParlayRMS.Utility.Data;

namespace ParlayRMS.DataAccess.Implement.TotalOutstanding
{
    public class TotalOutstandingDataAccess : BaseDataAccess
    {
        public List<RMS_Rpt_Parlay_Outstanding_Mem> RMS_Rpt_Parlay_Outstanding_Mem(long custId, string combo, string sportsType, string parlaysType)
        {
            var result = new List<RMS_Rpt_Parlay_Outstanding_Mem>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_Outstanding_Mem"))
            {
                db.AddInParameter("custid", DbType.Int64, custId);
                db.AddInParameter("selection", DbType.String, combo);
                db.AddInParameter("sporttype", DbType.String, sportsType);
                db.AddInParameter("ParlayType", DbType.String, parlaysType);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_Outstanding_Mem
                        {
                            TransId = reader.ToInt64("transid"),
                            Refno = reader.ToInt64("refno"),
                            TransDate = reader.ToDateTime("transdate"),
                            Odds = reader.ToDecimal("odds"),
                            Stake = reader.ToDecimal("stake"),
                            StakePt = reader.ToDecimal("stakept"),
                            MaxPayoutPt = reader.ToDecimal("net_win"),
                            TotalBet = reader.ToInt32("TotalBet"),
                            TotalSelection = Convert.ToInt32(reader.ToDecimal("TotalSelection")),
                            SportType = reader.ToInt32("sporttype")
                        });
                    }
                }
            }

            return result;
        }

        public List<RMS_Rpt_Parlay_Outstanding_Ag> RMS_Rpt_Parlay_Outstanding_Ag(long custId, string combo, string sportsType, string parlaysType)
        {
            var result = new List<RMS_Rpt_Parlay_Outstanding_Ag>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_Outstanding_Ag"))
            {
                db.AddInParameter("custid", DbType.Int64, custId);
                db.AddInParameter("selection", DbType.String, combo);
                db.AddInParameter("sporttype", DbType.String, sportsType);
                db.AddInParameter("ParlayType", DbType.String, parlaysType);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_Outstanding_Ag
                        {
                            CustId = reader.ToInt64("custid"),
                            Username = reader.ToString("username"),
                            StakePt = reader.ToDecimal("stakept"),
                            MaxPayoutPt = reader.ToDecimal("net_win"),
                            BetCount = reader.ToInt32("betcount")
                        });
                    }
                }
            }

            return result;
        }
    }
}