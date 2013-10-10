// --------------------------------------------------------------------------
// <copyright file="IChartsDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.Charts
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.Charts;

    public interface IChartsDataAccess
    {
        /// <summary>
        /// Gets monitor parlay summary data.
        /// </summary>
        /// <param name="bettype">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_Sum objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_Sum> RMS_Monitor_Parlay_Sum(int bettype, DateTime winlostDate, out long maxTransId);

        /// <summary>
        /// Gets top selections by ticket.
        /// </summary>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTicket> RMS_Monitor_Parlay_SelectionByTicket(DateTime winlostDate, long maxTransId);

        /// <summary>
        /// Gets selections by turnover.
        /// </summary>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover> RMS_Monitor_Parlay_SelectionByTurnover(DateTime winlostDate, long maxTransId);

        /// <summary>
        /// Gets top top leagues by selection and sport.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League> RMS_Monitor_Parlay_SelectionByTicket_League(
                                                                     int selection,
                                                                     int sportType,
                                                                     DateTime winlostDate,
                                                                     long maxTransId);

        /// <summary>
        /// Gets top leagues by selection and turnover.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_League objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League> RMS_Monitor_Parlay_SelectionByTurnover_League(
                                                                       int selection,
                                                                       int sportType,
                                                                       DateTime winlostDate,
                                                                       long maxTransId);

        /// <summary>
        /// Gets top currencies by selection and sport type.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_Currency objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency> RMS_Monitor_Parlay_SelectionByTicket_Currency(
                                                                        int selection,
                                                                        int sportType,
                                                                        DateTime winlostDate,
                                                                        long maxTransId);

        /// <summary>
        /// Get top currencies by selection and turnover.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_Currency objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionbyTurnover_Currency> RMS_Monitor_Parlay_SelectionByTurnover_Currency(
                                                                         int selection,
                                                                         int sportType,
                                                                         DateTime winlostDate,
                                                                         long maxTransId);

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
        IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event> RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event(
                                                                             int selection,
                                                                             int sportType,
                                                                             int currency,
                                                                             string topCurrency,
                                                                             DateTime winlostDate,
                                                                             long maxTransId);

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
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event> RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event(
                                                                               int selection,
                                                                               int sportType,
                                                                               int currency,
                                                                               string topCurrency,
                                                                               DateTime winlostDate,
                                                                               long maxTransId);

        /// <summary>
        /// Gets top events by top tickets and league.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_League_Event objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League_Event> RMS_Monitor_Parlay_SelectionByTicket_League_Event(
                                                                           int selection,
                                                                           int sportType,
                                                                           int leagueId,
                                                                           DateTime winlostDate,
                                                                           long maxTransId);

        /// <summary>
        /// Gets top events by top tickets and currency.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_League_Event objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League_Event> RMS_Monitor_Parlay_SelectionByTurnover_League_Event(
                                                                             int selection,
                                                                             int sportType,
                                                                             int leagueId,
                                                                             DateTime winlostDate,
                                                                             long maxTransId);

        /// <summary>
        /// Gets top events by top selection of turnover.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_Event objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event> RMS_Monitor_Parlay_SelectionByTurnover_Event(
                                                                      int selection,
                                                                      int sportType,
                                                                      DateTime winlostDate,
                                                                      long maxTransId);

        /// <summary>
        /// Gets top bet types by event.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_Event_BetType objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype> RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype(
                                                                              int selection,
                                                                              int sportType,
                                                                              int matchId,
                                                                              DateTime winlostDate,
                                                                              long maxTransId);

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
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice> RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice(
                                                                                     int selection,
                                                                                     int sportType,
                                                                                     int matchId,
                                                                                     int betType2,
                                                                                     DateTime winlostDate,
                                                                                     long maxTransId);

        /// <summary>
        /// Gets top events by top selections of ticket.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_Event objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event> RMS_Monitor_Parlay_SelectionByTicket_Event(
                                                                    int selection,
                                                                    int sportType,
                                                                    DateTime winlostDate,
                                                                    long maxTransId);

        /// <summary>
        /// Gets top bet types by event.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_Event_BetType objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype> RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype(
                                                                            int selection,
                                                                            int sportType,
                                                                            int matchId,
                                                                            DateTime winlostDate,
                                                                            long maxTransId);

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
        IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice> RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice(
                                                                                   int selection,
                                                                                   int sportType,
                                                                                   int matchId,
                                                                                   int betType2,
                                                                                   DateTime winlostDate,
                                                                                   long maxTransId);
    }
}