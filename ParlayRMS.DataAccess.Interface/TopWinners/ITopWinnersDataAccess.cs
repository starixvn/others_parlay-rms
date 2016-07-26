using ParlayRMS.DTO.BetList;
using ParlayRMS.DTO.TopWinners;
using System;
using System.Collections.Generic;

namespace ParlayRMS.DataAccess.Interface.TopWinners
{
    public interface ITopWinnersDataAccess
    {
        IEnumerable<RMS_Rpt_Parlay_TopWinner> RMS_Rpt_Parlay_TopWinners(
                                                  DateTime fromDate,
                                                  DateTime toDate,
                                                  DateTime moveBetsDay,
                                                  DateTime balanceUpDay,
                                                  string sportsType,
                                                  int selectTop,
                                                  string combos,
                                                  string parlayTypes);

        IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details(
                                                         long custId,
                                                         DateTime fromDate,
                                                         DateTime toDate,
                                                         DateTime moveBetsDay,
                                                         string sportsType,
                                                         string combos,
                                                         string parlayTypes);

        IEnumerable<RMS_Parlay_TransDetails> RMS_Rpt_Parlay_TopWinners_Bet_Details(long refNo);

        IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details_old(
                                                         long custId,
                                                         DateTime fromDate,
                                                         DateTime toDate,
                                                         string sportsType,
                                                         string combos,
                                                         string parlayTypes);

        BetInfo GetBetInfo(long transId);
    }
}