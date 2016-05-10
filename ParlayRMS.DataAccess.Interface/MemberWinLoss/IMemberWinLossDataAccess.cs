using ParlayRMS.DTO.MemberWinLoss;
using System;
using System.Collections.Generic;

namespace ParlayRMS.DataAccess.Interface.MemberWinLoss
{
    public interface IMemberWinLossDataAccess
    {
        IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> RMS_Rpt_Parlay_MemberWinLoss(
                                                      DateTime fromDate,
                                                      DateTime toDate,
                                                      DateTime moveBetsDay,
                                                      DateTime balanceUpDay,
                                                      string parlayTypes,
                                                      int sportsType,
                                                      string combos,
                                                      long custId);

        IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> RMS_Rpt_Parlay_AgentWinLoss(
                                                      DateTime fromDate,
                                                      DateTime toDate,
                                                      DateTime moveBetsDay,
                                                      DateTime balanceUpDay,
                                                      string parlayTypes,
                                                      int sportsType,
                                                      string combos,
                                                      long custId);
    }
}