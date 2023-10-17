namespace ParlayRMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Criteria.ViewIPInfo;
    using DTO.ViewIPInfo;

    public class ViewIPInfoDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();
        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> RMS_Rpt_Parlay_BetsStatisticsByIP(string ip, DateTime date)
        {
            var criteria = new Rpt_Parlay_BetsStatisticsByIP_Criteria
            {
                Ip = ip,
                FromDate = date
            };

            return repository.Fetch<RMS_Rpt_Parlay_ViewIPInfo>(criteria);
        }

        public RMS_Rpt_Parlay_UsersStatisticsByIP RMS_Rpt_Parlay_UsersStatisticsByIP(DateTime fromDate, int pageIndex, int pageSize, int totalItems, long maxTransId)
        {
            var criteria = new Rpt_Parlay_UsersStatisticsByIP_Criteria
            {
                FromDate = fromDate,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = totalItems,
                Max_TransId = maxTransId
            };

            var ipStats = repository.Fetch<RMS_Rpt_Parlay_UsersStatisticsByIP_IPStats>(criteria);
            var result = new RMS_Rpt_Parlay_UsersStatisticsByIP
            {
                IPStats = ipStats,
                TotalItems = criteria.TotalItems,
                MaxTransId = criteria.Max_TransId
            };

            return result;
        }

        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfoByUser> RMS_Rpt_Parlay_BetsStatisticsByIP_Sporttype(string ip, DateTime date, int userId, int sportType)
        {
            var criteria = new Rpt_ComboParlay_BetsStatisticsByIP_Sporttype_Criteria
            {
                FromDate = date,
                Ip = ip,
                CustId = userId,
                SportType = sportType
            };

            return repository.Fetch<RMS_Rpt_Parlay_ViewIPInfoByUser>(criteria);
        }

        public RMS_SystemParlay_CombinationDetails RMS_SystemParlay_CombinationDetails(long refNo)
        {
            var criteria = new SystemParlay_CombinationDetails_Criteria
            {
                RefNo = refNo
            };

            var (combos, comboRefs) = repository.FetchMultiple<RMS_SystemParlay_CombinationDetails_Combo, RMS_SystemParlay_CombinationDetails_ComboRef>(criteria);

            var result = new RMS_SystemParlay_CombinationDetails
            {
                Outstanding = criteria.Outstanding,
                Combo = combos,
                ComboRef = comboRefs
            };

            return result;
        }
    }

}
