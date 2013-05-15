// -----------------------------------------------------------------------
// <copyright file="TotalOutstandingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DataAccess.Implement.TotalOutstanding
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.TotalOutstanding;
    using ParlayRMS.DTO.TotalOutstanding;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TotalOutstandingDataAccess : BaseDataAccess, ITotalOutstandingDataAccess
    {
        public List<RMS_Rpt_Parlay_Outstanding_Mem> RMS_Rpt_Parlay_Outstanding_Mem(long custId, string combo, string sportsType)
        {
            var result = new List<RMS_Rpt_Parlay_Outstanding_Mem>();

            try
            {
                using (var db = new ObjectDb("RMS_Rpt_Parlay_Outstanding_Mem"))
                {
                    db.AddInParameter("custid", DbType.Int64, custId);
                    db.AddInParameter("selection", DbType.String, combo);
                    db.AddInParameter("sporttype", DbType.String, sportsType);

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
                                MaxPayoutPt = reader.ToDecimal("maxpayoutpt"),
                                Combo = reader.ToInt32("combo")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }

        public List<RMS_Rpt_Parlay_Outstanding_Ag> RMS_Rpt_Parlay_Outstanding_Ag(long custId, string combo, string sportsType)
        {
            var result = new List<RMS_Rpt_Parlay_Outstanding_Ag>();

            try
            {
                using (var db = new ObjectDb("RMS_Rpt_Parlay_Outstanding_Ag"))
                {
                    db.AddInParameter("custid", DbType.Int64, custId);
                    db.AddInParameter("selection", DbType.String, combo);
                    db.AddInParameter("sporttype", DbType.String, sportsType);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Rpt_Parlay_Outstanding_Ag
                            {
                                CustId = reader.ToInt64("custid"),
                                Username = reader.ToString("username"),
                                StakePt = reader.ToDecimal("stakept"),
                                MaxPayoutPt = reader.ToDecimal("maxpayoutpt"),
                                BetCount = reader.ToInt32("betcount")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }
    }
}