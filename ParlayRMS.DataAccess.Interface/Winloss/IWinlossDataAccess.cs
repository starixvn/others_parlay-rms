using ParlayRMS.DTO.Winloss;
using System;
using System.Collections.Generic;

namespace ParlayRMS.DataAccess.Interface.Winloss
{
    public interface IWinlossDataAccess
    {
        IEnumerable<RMS_Rpt_Parlay_Winloss> RMS_Rpt_Parlay_Winloss(
                                                DateTime fromDate,
                                                DateTime toDate,
                                                DateTime moveBetsDay,
                                                DateTime balanceUpDay,
                                                string parlayTypes,
                                                string sportsType,
                                                string combos,
                                                int reportType);
    }
}