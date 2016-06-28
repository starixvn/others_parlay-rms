// --------------------------------------------------------------------------
// <copyright file="ViewIPInfoService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.18">Created</change>
// </history>

namespace ParlayRMS.Services.Implement.ViewIPInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ParlayRMS.DataAccess.Implement.ViewIPInfo;
    using ParlayRMS.DataAccess.Interface.ViewIPInfo;
    using ParlayRMS.DTO.ViewIPInfo;
    using ParlayRMS.Models.ViewIpInfo;
    using ParlayRMS.Services.Interface.ViewIPInfo;

    /// <summary>
    /// Main service class for view-IP-info report and other related reports.
    /// </summary>
    public class ViewIPInfoService : BaseService, IViewIPInfoService
    {
        private readonly IViewIPInfoDataAccess _viewIPInfoDataAccess = new ViewIPInfoDataAccess();

        /// <summary>
        /// Get bets statistics base on specific IP and date.
        /// </summary>
        /// <param name="ip">Input IP value.</param>
        /// <param name="date">Input date value.</param>
        /// <returns>List of bets statistics.</returns>
        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> GetIPInfo(string ip, DateTime date)
        {
            return this._viewIPInfoDataAccess.RMS_Rpt_Parlay_BetsStatisticsByIP(ip, date);
        }

        /// <summary>
        /// Get IP Stat data.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="pageIndex">Page index value.</param>
        /// <param name="pageSize">Page size value.</param>
        /// <param name="totalItems">Total items value.</param>
        /// <param name="maxTransId">Maximum transaction ID value.</param>
        /// <returns>IP statistics.</returns>
        public RMS_Rpt_Parlay_UsersStatisticsByIP GetIPStats(DateTime fromDate, int pageIndex, int pageSize, int totalItems, long maxTransId)
        {
            return this._viewIPInfoDataAccess.RMS_Rpt_Parlay_UsersStatisticsByIP(fromDate, pageIndex, pageSize, totalItems, maxTransId);
        }

        /// <summary>
        /// Get bets statistics base on specific IP, date, user id and sport type.
        /// </summary>
        /// <param name="ip">Input IP value.</param>
        /// <param name="date">Input date value.</param>
        /// <param name="userId">Input user id value.</param>
        /// <param name="sportType">Input sport type value.</param>
        /// <returns>List of bets statistics.</returns>
        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfoByUser> GetIPInfoByUser(string ip, DateTime date, int userId, int sportType)
        {
            IEnumerable<RMS_Rpt_Parlay_ViewIPInfoByUser> records = this._viewIPInfoDataAccess.RMS_Rpt_Parlay_BetsStatisticsByIP_Sporttype(ip, date, userId, sportType);

            return records;
        }

        /// <summary>
        /// Get Parlay combination details.
        /// </summary>
        /// <param name="refNo">REFNO value.</param>
        /// <returns>List of combination details view model, group by date.</returns>
        public CombinationDetailsByDateViews GetBetCombinationDetails(long refNo)
        {
            //// Get records from DataAcess
            RMS_SystemParlay_CombinationDetails records = this._viewIPInfoDataAccess.RMS_SystemParlay_CombinationDetails(refNo);
            
            //// Convert to view model
            CombinationDetailsViews viewModel = MakeCombinationDetailsView(records);

            //// Convert to view model by date
            CombinationDetailsByDateViews viewModelGroupByDate = MakeCombinationDetailsByDateView(viewModel);
            return viewModelGroupByDate;
        }

        /// <summary>
        /// Transform CombinationDetails to view model.
        /// </summary>
        /// <param name="records">Combination Details records.</param>
        /// <returns>List of Combination Detail object in view model.</returns>
        private CombinationDetailsViews MakeCombinationDetailsView(RMS_SystemParlay_CombinationDetails records)
        {
            List<CombinationDetailsView> viewList = new List<CombinationDetailsView>();

            //// event info - a dictionary to get event detail by event serial
            Dictionary<int, List<CombinationDetailsView.EventItem>> eventInfo = new Dictionary<int, List<CombinationDetailsView.EventItem>>();

            //// make event info dictionary.
            //// add from ComboRef
            foreach (var item in records.ComboRef)
            {
                CombinationDetailsView.EventItem eventItem = new CombinationDetailsView.EventItem(item.HomeId, item.AwayId);
                if (!eventInfo.ContainsKey(item.Serial))
                {
                    eventInfo.Add(item.Serial, new List<CombinationDetailsView.EventItem>() { eventItem });
                }
                else
                {
                    eventInfo[item.Serial].Add(eventItem);
                }
            }
            //// add value for loose event
            CombinationDetailsView.EventItem loseEvent = new CombinationDetailsView.EventItem(-1, -1);
            eventInfo.Add(0, new List<CombinationDetailsView.EventItem>() { loseEvent });

            //// make CombinationDetailsView list
            foreach (var item in records.Combo)
            {
                CombinationDetailsView resultItem = new CombinationDetailsView();
                resultItem.Date = item.WinLostDate;
                resultItem.Events = eventInfo[item.Serial];
                resultItem.Odds = item.Modds;
                resultItem.Stake = item.Stake;
                resultItem.WinLoss = item.Winlost;
                resultItem.Status = item.StatusId;
                resultItem.StatusString = item.Status;

                viewList.Add(resultItem);
            }

            CombinationDetailsViews result = new CombinationDetailsViews();
            result.Outstanding = records.Outstanding;
            result.ViewList = viewList;
            return result;
        }

        /// <summary>
        /// Group Combination Detail view model by date.
        /// </summary>
        /// <param name="models">Combination Detail view model.</param>
        /// <returns>Combination Detail view model grouped by date.</returns>
        private CombinationDetailsByDateViews MakeCombinationDetailsByDateView(CombinationDetailsViews models)
        {
            // group by date
            var groupByDateViewList = models.ViewList.GroupBy(x => x.Date).ToDictionary(x => x.Key, x => x.ToList());

            CombinationDetailsByDateViews result = new CombinationDetailsByDateViews();
            result.Outstanding = models.Outstanding;
            result.ViewDict = groupByDateViewList;
            return result;
        }
    }
}