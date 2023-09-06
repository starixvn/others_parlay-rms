namespace ParlayRMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Criteria.MemberWinLoss;
    using DTO.MemberWinLoss;

    public class MemberWinLossDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> RMS_Rpt_Parlay_MemberWinLoss(
                                                             DateTime fromDate,
                                                             DateTime toDate,
                                                             DateTime moveBetsDay,
                                                             DateTime balanceUpDay,
                                                             string parlayTypes,
                                                             string sportsType,
                                                             byte liveType,
                                                             string combos,
                                                             int custId)
        {
            var criteria = new Rpt_ComboParlay_Winloss_Mem_Criteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                MoveBetsDay = moveBetsDay,
                BalanceUpDay = balanceUpDay,
                CustId = custId,
                SportType = sportsType,
                LiveStatus = liveType,
                Selection = combos,
                Parlaytype = parlayTypes
            };

            var winlosses = repository.Fetch<RMS_Rpt_Parlay_MemberWinLoss>(criteria).ToList();

            winlosses.ForEach(w =>
            {
                w.CustomerClass = string.IsNullOrEmpty(w.CustomerClass) ? "None" : w.CustomerClass;
            });
            
            return winlosses;
        }

        public IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> RMS_Rpt_Parlay_AgentWinLoss(
                                                             DateTime fromDate,
                                                             DateTime toDate,
                                                             DateTime moveBetsDay,
                                                             DateTime balanceUpDay,
                                                             string parlayTypes,
                                                             string sportsType,
                                                             byte liveType,
                                                             string combos,
                                                             List<int> custIds)
        {
            var criteria = new Rpt_ComboParlay_Winloss_Ag_Criteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                MoveBetsDay = moveBetsDay,
                BalanceUpDay = balanceUpDay,
                Recommends = string.Join(",", custIds),
                SportType = sportsType,
                LiveStatus = liveType,
                Selection = combos,
                Parlaytype = parlayTypes
            };

            var winlosses = repository.Fetch<RMS_Rpt_Parlay_MemberWinLoss>(criteria).ToList();

            winlosses.ForEach(w =>
            {
                w.CustomerClass = string.IsNullOrEmpty(w.CustomerClass) ? "None" : w.CustomerClass;
            });

            return winlosses;
        }
    }
}
