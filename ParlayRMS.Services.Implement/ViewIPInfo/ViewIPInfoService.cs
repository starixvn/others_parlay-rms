// --------------------------------------------------------------------------
// <copyright file="ViewIPInfoService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
        public IDictionary<DateTime, List<CombinationDetailsView>> GetBetCombinationDetails(long refNo)
        {
            //Get records from DataAcess
            RMS_SystemParlay_CombinationDetails records = this._viewIPInfoDataAccess.RMS_SystemParlay_CombinationDetails(refNo);

            //Convert to view model
            IEnumerable<CombinationDetailsView> viewModel = MakeCombinationDetailsView(records);

            //Convert to view model by date
            IDictionary<DateTime, List<CombinationDetailsView>> viewModelGroupByDate = MakeCombinationDetailsByDateView(viewModel);
            return viewModelGroupByDate;
        }

        /// <summary>
        /// Transform CombinationDetails to view model
        /// </summary>
        /// <param name="records">Combination Details records.</param>
        /// <returns>List of Combination Detail object in view model.</returns>
        private IEnumerable<CombinationDetailsView> MakeCombinationDetailsView(RMS_SystemParlay_CombinationDetails records)
        {
            List<CombinationDetailsView> result = new List<CombinationDetailsView>();
            if (records == null) return result;

            // event info - a dictionary to get event detail by event serial
            Dictionary<int, List<CombinationDetailsView.EventItem>> EventInfo = new Dictionary<int, List<CombinationDetailsView.EventItem>>();

            //make event info dictionary.
            // add from ComboRef
            foreach (var item in records.ComboRef)
            {
                CombinationDetailsView.EventItem EventItem = new CombinationDetailsView.EventItem(item.HomeId, item.AwayId);
                if (!EventInfo.ContainsKey(item.Serial))
                {
                    EventInfo.Add(item.Serial, new List<CombinationDetailsView.EventItem>() { EventItem });
                }
                else
                {
                    EventInfo[item.Serial].Add(EventItem);
                }
            }
            // add value for loose event
            CombinationDetailsView.EventItem loseEvent = new CombinationDetailsView.EventItem(-1, -1);
            EventInfo.Add(0, new List<CombinationDetailsView.EventItem>() { loseEvent });

            // make CombinationDetailsView list
            foreach (var item in records.Combo)
            {
                CombinationDetailsView ResultItem = new CombinationDetailsView();
                ResultItem.Date = item.WinLostDate;
                ResultItem.Events = EventInfo[item.Serial];
                ResultItem.Odds = item.Modds;
                ResultItem.Stake = item.Stake;
                ResultItem.WinLoss = item.Winlost;
                ResultItem.Status = item.StatusId;
                ResultItem.StatusString = item.Status;

                result.Add(ResultItem);
            }

            return result;
        }

        /// <summary>
        /// Group Combination Detail view model by date.
        /// </summary>
        /// <param name="models">Combination Detail view model</param>
        /// <returns>Combination Detail view model grouped by date</returns>
        private IDictionary<DateTime, List<CombinationDetailsView>> MakeCombinationDetailsByDateView(IEnumerable<CombinationDetailsView> models)
        {
            // group by date
            var result = models.GroupBy(x => x.Date).ToDictionary(x => x.Key, x => x.ToList());

            return result;
        }
    }
}