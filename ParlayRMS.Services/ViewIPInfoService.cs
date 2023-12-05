namespace ParlayRMS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ParlayRMS.DataAccess;
    using ParlayRMS.DTO.ViewIPInfo;
    using ParlayRMS.Models.ViewIpInfo;

    public class ViewIPInfoService : BaseService
    {
        private readonly ViewIPInfoDataAccess _viewIPInfoDataAccess = new ViewIPInfoDataAccess();

        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> GetIPInfo(string ip, DateTime date, string ticketStatus)
        {
            return _viewIPInfoDataAccess.RMS_Rpt_Parlay_BetsStatisticsByIP(ip, date, ticketStatus);
        }

        public RMS_Rpt_Parlay_UsersStatisticsByIP GetIPStats(DateTime fromDate, int pageIndex, int pageSize, int totalItems, long maxTransId, string ticketStatus)
        {
            return _viewIPInfoDataAccess.RMS_Rpt_Parlay_UsersStatisticsByIP(fromDate, pageIndex, pageSize, totalItems, maxTransId, ticketStatus);
        }

        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfoByUser> GetIPInfoByUser(string ip, DateTime date, int userId, int sportType, string ticketStatus)
        {
            var records = _viewIPInfoDataAccess.RMS_Rpt_Parlay_BetsStatisticsByIP_Sporttype(ip, date, userId, sportType, ticketStatus);

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
                var resultItem = new CombinationDetailsView
                {
                    Date = item.WinLostDate,
                    Events = eventInfo[item.Serial],
                    Odds = item.Modds,
                    Stake = item.Stake,
                    WinLoss = item.Winlost,
                    Status = item.StatusId,
                    StatusString = item.Status,
                    IsBoostBonus = item.IsBoostBonus,
                    OriginalOdds = item.OriginalOdds,
                    BoostOdds = item.BoostOdds,
                    BoostRate = item.BoostRate,
                    Bba = item.Bba
                };

                viewList.Add(resultItem);
            }

            var result = new CombinationDetailsViews
            {
                Outstanding = records.Outstanding,
                ViewList = viewList
            };
            return result;
        }

        private CombinationDetailsByDateViews MakeCombinationDetailsByDateView(CombinationDetailsViews models)
        {
            var groupByDateViewList = models.ViewList.GroupBy(x => x.Date).ToDictionary(x => x.Key, x => x.ToList());

            var result = new CombinationDetailsByDateViews
            {
                Outstanding = models.Outstanding,
                ViewDict = groupByDateViewList
            };
            return result;
        }
    }
}
