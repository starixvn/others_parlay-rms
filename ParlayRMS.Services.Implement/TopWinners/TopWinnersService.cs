// --------------------------------------------------------------------------
// <copyright file="TopWinnersService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.TopWinners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ParlayRMS.DataAccess.Implement.TopWinners;
    using ParlayRMS.DataAccess.Interface.TopWinners;
    using ParlayRMS.DTO.BetList;
    using ParlayRMS.DTO.TopWinners;
    using ParlayRMS.Services.Implement.Common;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Interface.TopWinners;

    /// <summary>
    /// Top Winners Service.
    /// </summary>
    public class TopWinnersService : BaseService, ITopWinnersService
    {
        private readonly ITopWinnersDataAccess _topwinnersDataAccess = new TopWinnersDataAccess();
        private readonly ICommonService _commonService = new CommonService();

        /// <summary>
        /// Gets top winners.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="sportsType">Type of the sports.</param>
        /// <param name="selectTop">Number of top items.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>List of top winners.</returns>
        public IEnumerable<RMS_Rpt_Parlay_TopWinner> GetTopWinners(
                                                         DateTime fromDate, 
                                                         DateTime toDate, 
                                                         int sportsType, 
                                                         int selectTop, 
                                                         string combos)
        {
            var info = _commonService.GetBalanceUpDayInfo();
            return this._topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners(fromDate, toDate, info.MoveBetsDay, info.BalanceUpDay, sportsType, selectTop, combos);
        }

        /// <summary>
        /// Gets bet detail.
        /// </summary>
        /// <param name="refNo">Reference number.</param>
        /// <returns>Transaction detail.</returns>
        public IEnumerable<RMS_Parlay_TransDetails> GetBetDetail(long refNo)
        {
            return this._topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Bet_Details(refNo);
        }

        /// <summary>
        /// Gets top winner details.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="sportsType">Sport type values.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>Detail of a top winner.</returns>
        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> GetTopWinnerDetails(long custId, DateTime fromDate, DateTime toDate, int sportsType, string combos)
        {
            var info = _commonService.GetBalanceUpDayInfo();

            if (info.BalanceUpDay.CompareTo(fromDate) <= 0 && info.BalanceUpDay.CompareTo(toDate) <= 0)
            {
                return this._topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details(custId, fromDate, toDate, info.MoveBetsDay, sportsType, combos);
            }
            else if (info.BalanceUpDay.CompareTo(fromDate) > 0 && info.BalanceUpDay.CompareTo(toDate) > 0)
            {
                return this._topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details_old(custId, fromDate, toDate, sportsType, combos);
            }
            else
            {
                IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> listEntity_old = null;
                IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> listEntity = null;

                listEntity_old = this._topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details_old(custId, fromDate, info.BalanceUpDay.AddDays(-1), sportsType, combos);
                listEntity = this._topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details(custId, info.BalanceUpDay, toDate, info.MoveBetsDay, sportsType, combos);

                if (listEntity_old.Count() != 0 && listEntity.Count() != 0)
                {
                    return listEntity.Union(listEntity_old);
                }
                else if (listEntity_old.Count() != 0 && listEntity.Count() <= 0)
                {
                    return listEntity_old;
                }
                else
                {
                    return listEntity;
                }
            }
        }
    }
}