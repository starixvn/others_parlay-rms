// --------------------------------------------------------------------------
// <copyright file="ChartsService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.Charts
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DataAccess.Implement.Charts;
    using ParlayRMS.DTO.Charts;

    /// <summary>
    /// Charts Service.
    /// </summary>
    public class ChartsService : BaseService, IChartsService
    {
        private readonly ChartsDataAccess _monitorDataAccess = new ChartsDataAccess();

        #region SummaryData

        /// <summary>
        /// Gets summary data.
        /// </summary>
        /// <param name="bettype">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_Sum objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_Sum> GetSumDataResult(int bettype, DateTime winlostDate, out long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_Sum(bettype, winlostDate, out maxTransId);
        }

        #endregion SummaryData

        #region ByTicket

        /// <summary>
        /// Gets top selections by ticket.
        /// </summary>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket> GetTopSelectionByTicket(DateTime winlostDate, long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket(winlostDate, maxTransId);
        }

        /// <summary>
        /// Gets top top leagues by selection and sport.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League> GetTopLeagueByTicket(
                                                                            int selection,
                                                                            int sportType,
                                                                            DateTime winlostDate,
                                                                            long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket_League(selection, sportType, winlostDate, maxTransId);
        }

        /// <summary>
        /// Gets top currencies by selection and sport type.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_Currency objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency> GetTopCurrencyByTicket(
                                                                              int selection,
                                                                              int sportType,
                                                                              DateTime winlostDate,
                                                                              long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket_Currency(selection, sportType, winlostDate, maxTransId);
        }

        #endregion ByTicket

        #region ByTurnover

        /// <summary>
        /// Gets selections by turnover.
        /// </summary>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover> GetTopSelectionByTurnover(DateTime winlostDate, long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover(winlostDate, maxTransId);
        }

        /// <summary>
        /// Gets top leagues by selection and turnover.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_League objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League> GetTopLeagueByTurnover(
                                                                              int selection,
                                                                              int sportType,
                                                                              DateTime winlostDate,
                                                                              long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_League(selection, sportType, winlostDate, maxTransId);
        }

        /// <summary>
        /// Get top currencies by selection and turnover.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_Currency objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTurnover_Currency> GetTopCurrencyByTurnover(
                                                                                int selection,
                                                                                int sportType,
                                                                                DateTime winlostDate,
                                                                                long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_Currency(selection, sportType, winlostDate, maxTransId);
        }

        #endregion ByTurnover

        #region ByCurrency

        /// <summary>
        /// Gets top events by top tickets and currency.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="currency">Currency ID.</param>
        /// <param name="topCurrency">Top currency.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_Currency_Event objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event> GetTopSelectionTicketEventByCurrency(
                                                                                    int selection,
                                                                                    int sportType,
                                                                                    int currency,
                                                                                    string topCurrency,
                                                                                    DateTime winlostDate,
                                                                                    long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event(selection, sportType, currency, topCurrency, winlostDate, maxTransId);
        }

        /// <summary>
        /// Gets top events by top turnovers and currency.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="currency">Currency ID.</param>
        /// <param name="topCurrency">Top currency.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event> GetTopSelectionTurnoverEventByCurrency(
                                                                                      int selection,
                                                                                      int sportType,
                                                                                      int currency,
                                                                                      string topCurrency,
                                                                                      DateTime winlostDate,
                                                                                      long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event(selection, sportType, currency, topCurrency, winlostDate, maxTransId);
        }

        #endregion ByCurrency

        #region ByLeague

        /// <summary>
        /// Gets top events by top tickets and league.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_League_Event objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League_Event> GetTopSelectionTicketEventByLeague(
                                                                                  int selection,
                                                                                  int sportType,
                                                                                  int leagueId,
                                                                                  DateTime winlostDate,
                                                                                  long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket_League_Event(selection, sportType, leagueId, winlostDate, maxTransId);
        }

        /// <summary>
        /// Gets top events by top tickets and currency.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_League_Event objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League_Event> GetTopSelectionTurnoverEventByLeague(
                                                                                    int selection,
                                                                                    int sportType,
                                                                                    int leagueId,
                                                                                    DateTime winlostDate,
                                                                                    long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_League_Event(selection, sportType, leagueId, winlostDate, maxTransId);
        }

        #endregion ByLeague

        #region Event

        /// <summary>
        /// Gets top events by top selection of turnover.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_Event objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event> GetTopSelectionByTurnover_Event(
                                                                             int selection,
                                                                             int sportType,
                                                                             DateTime winlostDate,
                                                                             long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_Event(selection, sportType, winlostDate, maxTransId);
        }

        /// <summary>
        /// Gets top bet types by event.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_Event_BetType objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype> GetTopSelectionByTurnover_Event_Bettype(
                                                                                     int selection,
                                                                                     int sportType,
                                                                                     int matchId,
                                                                                     DateTime winlostDate,
                                                                                     long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype(selection, sportType, matchId, winlostDate, maxTransId);
        }

        /// <summary>
        /// Gets report favorite choices by event and bet type.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="betType2">Bet type ID.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_Event_BetType_Choice objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice> GetTopSelectionByTurnover_Event_Bettype_Choice(
                                                                                            int selection,
                                                                                            int sportType,
                                                                                            int matchId,
                                                                                            int betType2,
                                                                                            DateTime winlostDate,
                                                                                            long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice(selection, sportType, matchId, betType2, winlostDate, maxTransId);
        }

        /// <summary>
        /// Gets top events by top selections of ticket.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_Event objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event> GetTopSelectionByTicket_Event(
                                                                           int selection,
                                                                           int sportType,
                                                                           DateTime winlostDate,
                                                                           long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket_Event(selection, sportType, winlostDate, maxTransId);
        }

        /// <summary>
        /// Gets top bet types by event.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_Event_BetType objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype> GetTopSelectionByTicket_Event_Bettype(
                                                                                   int selection,
                                                                                   int sportType,
                                                                                   int matchId,
                                                                                   DateTime winlostDate,
                                                                                   long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype(selection, sportType, matchId, winlostDate, maxTransId);
        }

        /// <summary>
        /// Gets report favorite choices by event and bet type.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="betType2">Bet type ID.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_Event_BetType_Choice objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice> GetTopSelectionByTicket_Event_Bettype_Choice(
                                                                                         int selection,
                                                                                         int sportType,
                                                                                         int matchId,
                                                                                         int betType2,
                                                                                         DateTime winlostDate,
                                                                                         long maxTransId)
        {
            return this._monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice(selection, sportType, matchId, betType2, winlostDate, maxTransId);
        }

        #endregion Event
    }
}