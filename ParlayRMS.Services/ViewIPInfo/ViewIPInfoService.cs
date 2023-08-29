namespace ParlayRMS.Services.ViewIPInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.ViewIPInfo;
    using ParlayRMS.DTO.ViewIPInfo;
    using Models.ViewIpInfo;

    public class ViewIPInfoService : BaseService
    {
        private readonly ViewIPInfoDataAccess _viewIPInfoDataAccess = new ViewIPInfoDataAccess();

        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> GetIPInfo(string ip, DateTime date)
        {
            return _viewIPInfoDataAccess.RMS_Rpt_Parlay_BetsStatisticsByIP(ip, date);
        }

        public RMS_Rpt_Parlay_UsersStatisticsByIP GetIPStats(DateTime fromDate, int pageIndex, int pageSize, int totalItems, long maxTransId)
        {
            return _viewIPInfoDataAccess.RMS_Rpt_Parlay_UsersStatisticsByIP(fromDate, pageIndex, pageSize, totalItems, maxTransId);
        }

        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfoByUser> GetIPInfoByUser(string ip, DateTime date, int userId, int sportType)
        {
            var records = _viewIPInfoDataAccess.RMS_Rpt_Parlay_BetsStatisticsByIP_Sporttype(ip, date, userId, sportType);

            return records;
        }

        public CombinationDetailsByDateViews GetBetCombinationDetails(long refNo)
        {
            var records = _viewIPInfoDataAccess.RMS_SystemParlay_CombinationDetails(refNo);

            var viewModel = MakeCombinationDetailsView(records);

            var viewModelGroupByDate = MakeCombinationDetailsByDateView(viewModel);
            return viewModelGroupByDate;
        }

        private CombinationDetailsViews MakeCombinationDetailsView(RMS_SystemParlay_CombinationDetails records)
        {
            var viewList = new List<CombinationDetailsView>();

            var eventInfo = new Dictionary<int, List<CombinationDetailsView.EventItem>>();

            foreach (var item in records.ComboRef)
            {
                var eventItem = new CombinationDetailsView.EventItem(item.HomeId, item.AwayId);
                if (!eventInfo.ContainsKey(item.Serial))
                {
                    eventInfo.Add(item.Serial, new List<CombinationDetailsView.EventItem> { eventItem });
                }
                else
                {
                    eventInfo[item.Serial].Add(eventItem);
                }
            }

            var loseEvent = new CombinationDetailsView.EventItem(-1, -1);
            eventInfo.Add(0, new List<CombinationDetailsView.EventItem> { loseEvent });

            foreach (var item in records.Combo)
            {
                var resultItem = new CombinationDetailsView();
                resultItem.Date = item.WinLostDate;
                resultItem.Events = eventInfo[item.Serial];
                resultItem.Odds = item.Modds;
                resultItem.Stake = item.Stake;
                resultItem.WinLoss = item.Winlost;
                resultItem.Status = item.StatusId;
                resultItem.StatusString = item.Status;

                viewList.Add(resultItem);
            }

            var result = new CombinationDetailsViews();
            result.Outstanding = records.Outstanding;
            result.ViewList = viewList;
            return result;
        }

        private CombinationDetailsByDateViews MakeCombinationDetailsByDateView(CombinationDetailsViews models)
        {
            var groupByDateViewList = models.ViewList.GroupBy(x => x.Date).ToDictionary(x => x.Key, x => x.ToList());

            var result = new CombinationDetailsByDateViews();
            result.Outstanding = models.Outstanding;
            result.ViewDict = groupByDateViewList;
            return result;
        }
    }
}
