// -----------------------------------------------------------------------
// <copyright file="WinlossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DataAccess.Implement.Winloss
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.Winloss;
    using ParlayRMS.DTO.Winloss;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinlossDataAccess : BaseDataAccess, IWinlossDataAccess
    {
        public IEnumerable<RMS_Rpt_Parlay_Winloss> RMS_Rpt_Parlay_Winloss(DateTime fromDate, DateTime toDate, DateTime moveBetsDay, DateTime balanceUpDay, int betType)
        {
            var result = new List<RMS_Rpt_Parlay_Winloss>();

            try
            {
                using (var db = new ObjectDb("RMS_Rpt_Parlay_Winloss"))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                    db.AddInParameter("todate", DbType.DateTime, toDate);
                    db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
                    db.AddInParameter("balanceupday", DbType.DateTime, balanceUpDay);
                    db.AddInParameter("bettype", DbType.Int32, betType);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Rpt_Parlay_Winloss
                            {
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
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }
    }
}