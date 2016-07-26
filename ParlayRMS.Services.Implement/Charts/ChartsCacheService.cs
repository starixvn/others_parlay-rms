// --------------------------------------------------------------------------
// <copyright file="ChartsCacheService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.Charts
{
    using System;
    using System.Collections.Generic;
    using Fanex.Caching;
    using ParlayRMS.DTO.Charts;
    using ParlayRMS.Models.Charts;
    using ParlayRMS.Services.Interface.Charts;
    using ParlayRMS.Web.Session;

    /// <summary>
    /// Charts Cache Service.
    /// </summary>
    public class ChartsCacheService : BaseService, IChartsService
    {
        public static readonly bool IsCache = Utility.AppSetting.IsCache;

        private const int RefreshCache = 10; // 10s
        private const string ByTicket = "BYTICKET";
        private const string ByTurnover = "BYTURNOVER";
        private const string TopSelection = "TOPSELECTION";
        private const string ByCurrency = "BYCURRENCY";
        private const string ByLeague = "BYLEAGUE";
        private const string ByEvent = "BYEVENT";

        private const int ExpiredCache = 60; // 60s

        private static readonly ICache CacheServiceInstance = CacheService.CreateInstance(CacheType.MemoryCache);
        private static readonly IChartsService ChartsServiceInstance = new ChartsService();

        private static object locker = new object();

        private static DateTime lastTimeSummary = DateTime.Now;
        private static long maxTransId = 0;

        public static long MaxTransId
        {
            get
            {
                return maxTransId;
            }

            set
            {
                maxTransId = value;
            }
        }

        #region SummaryData

        /// <summary>
        /// Gets summary data result.
        /// </summary>
        /// <param name="bettype">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_Sum object.</returns>
        public IEnumerable<RMS_Monitor_Parlay_Sum> GetSumDataResult(int bettype, DateTime winlostDate, out long maxTransId)
        {
            maxTransId = ChartsCacheService.MaxTransId;

            if (!ChartsCacheService.IsCache)
            {
                var result = ChartsServiceInstance.GetSumDataResult(bettype, winlostDate, out maxTransId);
                ChartsCacheService.MaxTransId = maxTransId;
                return result;
            }

            return this.GetSummaryCache(bettype, winlostDate).Sum;
        }

        #endregion SummaryData

        #region ByTicket

        /// <summary>
        /// Get Top Selections By Ticket.
        /// </summary>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket> GetTopSelectionByTicket(DateTime winlostDate, long maxTransId)
        {
            return this.GetSummaryCache(9, winlostDate).SelectionByTicket;
        }

        /// <summary>
        /// Get Top Leagues By Ticket.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_League objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League> GetTopLeagueByTicket(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTicketCache(selection, sportType, winlostDate).SelectionByTicket_League;
        }

        /// <summary>
        /// Get Top Currencies By Ticket.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_Currency objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency> GetTopCurrencyByTicket(
                                                                              int selection, 
                                                                              int sportType, 
                                                                              DateTime winlostDate, 
                                                                              long maxTransId)
        {
            return this.GetTopSelectionByTicketCache(selection, sportType, winlostDate).SelectionbyTicket_Currency;
        }

        #endregion ByTicket

        #region ByTurnover

        /// <summary>
        /// Get Top Selections By Turnover.
        /// </summary>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover> GetTopSelectionByTurnover(DateTime winlostDate, long maxTransId)
        {
            return this.GetSummaryCache(9, winlostDate).SelectionByTurnover;
        }

        /// <summary>
        /// Get Top Leagues By Turnover.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_League objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League> GetTopLeagueByTurnover(
                                                                              int selection, 
                                                                              int sportType, 
                                                                              DateTime winlostDate, 
                                                                              long maxTransId)
        {
            return this.GetTopSelectionByTurnoverCache(selection, sportType, winlostDate).SelectionByTurnover_League;
        }

        /// <summary>
        /// Get Top Currencies By Turnover.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_Currency objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTurnover_Currency> GetTopCurrencyByTurnover(
                                                                                int selection, 
                                                                                int sportType, 
                                                                                DateTime winlostDate, 
                                                                                long maxTransId)
        {
            return this.GetTopSelectionByTurnoverCache(selection, sportType, winlostDate).SelectionbyTurnover_Currency;
        }

        #endregion ByTurnover

        #region ByCurrency

        /// <summary>
        /// Get Top Selection, Ticket and Event By Currency.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="currency">Currency ID.</param>
        /// <param name="topCurrency">Top Currencies.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_Currency_Event objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event> GetTopSelectionTicketEventByCurrency(
                                                                                    int selection, 
                                                                                    int sportType, 
                                                                                    int currency, 
                                                                                    string topCurrency, 
                                                                                    DateTime winlostDate, 
                                                                                    long maxTransId)
        {
            return this.GetTopSelectionByTicketEventByCurrencyCache(selection, sportType, currency, topCurrency, winlostDate).SelectionbyTicketEventByCurrency;
        }

        /// <summary>
        /// Gets top events by top turnovers and currency from cache.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="currency">Currency ID.</param>
        /// <param name="topCurrency">Top currency.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event> GetTopSelectionTurnoverEventByCurrency(
                                                                                      int selection, 
                                                                                      int sportType, 
                                                                                      int currency, 
                                                                                      string topCurrency, 
                                                                                      DateTime winlostDate, 
                                                                                      long maxTransId)
        {
            return this.GetTopSelectionByTurnoverEventByCurrencyCache(selection, sportType, currency, topCurrency, winlostDate).SelectionbyTurnoverEventByCurrency;
        }

        #endregion ByCurrency

        #region ByLeague

        /// <summary>
        /// Gets top events by top tickets and league from memory cache.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_League_Event objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League_Event> GetTopSelectionTicketEventByLeague(
                                                                                  int selection, 
                                                                                  int sportType, 
                                                                                  int leagueId, 
                                                                                  DateTime winlostDate, 
                                                                                  long maxTransId)
        {
            return this.GetTopSelectionByTicketEventByLeagueCache(selection, sportType, leagueId, winlostDate).SelectionbyTicketEventByLeague;
        }

        /// <summary>
        /// Get Top Selection, Turnover and Event By League.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_League_Event objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League_Event> GetTopSelectionTurnoverEventByLeague(
                                                                                    int selection, 
                                                                                    int sportType, 
                                                                                    int leagueId, 
                                                                                    DateTime winlostDate, 
                                                                                    long maxTransId)
        {
            return this.GetTopSelectionByTurnoverEventByLeagueCache(selection, sportType, leagueId, winlostDate).SelectionbyTurnoverEventByLeague;
        }

        #endregion ByLeague

        #region Event

        /// <summary>
        /// Get Top Selection By Turnover and Event.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_Event objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event> GetTopSelectionByTurnover_Event(
                                                                             int selection, 
                                                                             int sportType, 
                                                                             DateTime winlostDate, 
                                                                             long maxTransId)
        {
            return this.GetTopSelectionByTurnoverEventCache(selection, sportType, winlostDate).SelectionByTurnoverEvent;
        }

        /// <summary>
        /// Get Top Selection By Turnover, Event and Bet type.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="matchId">The Match ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTurnover_Event_BetType objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype> GetTopSelectionByTurnover_Event_Bettype(
                                                                                    int selection, 
                                                                                    int sportType, 
                                                                                    int matchId, 
                                                                                    DateTime winlostDate, 
                                                                                    long maxTransId)
        {
            return this.GetTopSelectionByTuroverBettypeCache(selection, sportType, matchId, winlostDate).SelectionByTurnoverEventBettype;
        }

        /// <summary>
        /// Get Top Selections By Turnover, Event, Bet Type and Choice.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="matchId">The Match ID.</param>
        /// <param name="betType2">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
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
            return this.GetTopSelectionByTurnoverEventBettypeChoiceCache(selection, sportType, matchId, betType2, winlostDate).SelectionByTurnoverEventBettypeChoice;
        }

        /// <summary>
        /// Get Top Selections By Ticket and Event.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_Event objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event> GetTopSelectionByTicket_Event(
                                                                            int selection, 
                                                                            int sportType, 
                                                                            DateTime winlostDate, 
                                                                            long maxTransId)
        {
            return this.GetTopSelectionByTicketEventCache(selection, sportType, winlostDate).SelectionByTicketEvent;
        }

        /// <summary>
        /// Get Top Selection By Ticket, Event and Bet type.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="matchId">The Match ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="maxTransId">Maximum Transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_SelectionByTicket_Event_BetType objects.</returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype> GetTopSelectionByTicket_Event_Bettype(
                                                                                    int selection, 
                                                                                    int sportType, 
                                                                                    int matchId, 
                                                                                    DateTime winlostDate, 
                                                                                    long maxTransId)
        {
            return this.GetTopSelectionByTicketEventBettypeCache(selection, sportType, matchId, winlostDate).SelectionByTicketEventBettype;
        }

        /// <summary>
        /// Get Top Selection By Ticket, Event, Bet type and Choice.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="matchId">The Match ID.</param>
        /// <param name="betType2">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
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
            return this.GetTopSelectionByTicketEventBettypeChoiceCache(selection, sportType, matchId, betType2, winlostDate).SelectionByTicketEventBettypeChoice;
        }

        #endregion Event

        /// <summary>
        /// Set cache.
        /// </summary>
        /// <param name="bettype">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="isTicket">Is get by ticket.</param>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        public void SetCache(int bettype, DateTime winlostDate, bool isTicket, int selection, int sportType)
        {
            if (!IsCache)
            {
                return;
            }

            this.SetSummaryCache(bettype, winlostDate);
            if (selection != 0)
            {
                if (isTicket)
                {
                    this.SetTopSelectionByTicketCache(selection, sportType, winlostDate);
                }
                else
                {
                    this.SetTopSelectionByTurnoverCache(selection, sportType, winlostDate);
                }
            }
        }

        /// <summary>
        /// Set cache.
        /// </summary>
        /// <param name="bettype">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="isTicket">Is get by ticket.</param>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="currency">Currency ID.</param>
        /// <param name="topCurrency">Top Currencies.</param>
        /// <param name="leagueId">League ID.</param>
        public void SetCache(
                        int bettype, 
                        DateTime winlostDate, 
                        bool isTicket, 
                        int selection, 
                        int sportType, 
                        int currency, 
                        string topCurrency, 
                        int leagueId)
        {
            if (!IsCache)
            {
                return;
            }

            this.SetSummaryCache(bettype, winlostDate);
            if (selection != 0)
            {
                if (isTicket)
                {
                    this.SetTopSelectionByTicketCache(selection, sportType, winlostDate);
                    if (currency != -1 || leagueId != 0)
                    {
                        this.SetCacheEvent(selection, sportType, currency, topCurrency, leagueId, winlostDate, true);
                    }
                }
                else
                {
                    this.SetTopSelectionByTurnoverCache(selection, sportType, winlostDate);
                    if (currency != -1 || leagueId != 0)
                    {
                        this.SetCacheEvent(selection, sportType, currency, topCurrency, leagueId, winlostDate, false);
                    }
                }
            }
        }

        /// <summary>
        /// Set Summary Cache.
        /// </summary>
        /// <param name="bettype">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetSummaryCache(int bettype, DateTime winlostDate)
        {
            if (!IsCache)
            {
                return;
            }

            string key = TopSelection + bettype;

            if (DateTime.Now.Subtract(lastTimeSummary) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(lastTimeSummary) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        var result = GetSummary(bettype, winlostDate);
                        CacheServiceInstance.Set<ChartSummaryCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                        lastTimeSummary = DateTime.Now;
                    }
                }
            }
        }

        /// <summary>
        /// Set Cache Event.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="currency">Currency ID.</param>
        /// <param name="topCurrency">Top Currencies.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="isTicket">Is get by ticket.</param>
        public void SetCacheEvent(int selection, int sportType, int currency, string topCurrency, int leagueId, DateTime winlostDate, bool isTicket)
        {
            if (!IsCache)
            {
                return;
            }

            if (currency != -1)
            {
                if (isTicket) 
                {
                    this.SetTopSelectionByTicketEventByCurrencyCache(selection, sportType, currency, topCurrency, winlostDate);
                }
                else 
                {
                    this.SetTopSelectionByTurnoverEventByCurrencyCache(selection, sportType, currency, topCurrency, winlostDate);
                }
            }
            else 
            {
                if (isTicket) 
                {
                    this.SetTopSelectionByTicketEventByLeagueCache(selection, sportType, leagueId, winlostDate);
                }
                else 
                {
                    this.SetTopSelectionByTurnoverEventByLeagueCache(selection, sportType, leagueId, winlostDate);
                }
            }
        }

        /// <summary>
        /// Set Cache Bet type.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="matchId">The Match ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="isTicket">Is get by ticket.</param>
        public void SetCacheBettype(int selection, int sportType, int matchId, DateTime winlostDate, bool isTicket)
        {
            if (!IsCache)
            {
                return;
            }

            if (matchId != 0)
            {
                if (isTicket)
                {
                    this.SetTopSelectionByTicketEventBettypeCache(selection, sportType, matchId, winlostDate);
                }
                else
                {
                    this.SetTopSelectionByTurnoverBettypeCache(selection, sportType, matchId, winlostDate);
                }
            }
        }

        /// <summary>
        /// Set Cache Bet type Choice.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="matchId">The Match ID.</param>
        /// <param name="betType2">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <param name="isTicket">Is get by ticket.</param>
        public void SetCacheBettypeChoice(int selection, int sportType, int matchId, int betType2, DateTime winlostDate, bool isTicket)
        {
            if (!IsCache)
            {
                return;
            }

            if (matchId != 0 && betType2 != 0)
            {
                if (isTicket)
                {
                    this.SetTopSelectionByTicketEventBettypeChoiceCache(selection, sportType, matchId, betType2, winlostDate);
                }
                else
                {
                    this.SetTopSelectionByTurnoverEventBettypeChoiceCache(selection, sportType, matchId, betType2, winlostDate);
                }
            }
        }

        /// <summary>
        /// Get summary data.
        /// </summary>
        /// <param name="bettype">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartSummaryCache object contains the summary data.</returns>
        public ChartSummaryCache GetSummary(int bettype, DateTime winlostDate)
        {
            long newMaxTransId = 0;
            var sumData = ChartsServiceInstance.GetSumDataResult(bettype, winlostDate, out newMaxTransId);
            maxTransId = newMaxTransId;
            var ticketData = ChartsServiceInstance.GetTopSelectionByTicket(winlostDate, maxTransId);
            var turnoverData = ChartsServiceInstance.GetTopSelectionByTurnover(winlostDate, maxTransId);
            var result = new ChartSummaryCache
            {
                Sum = sumData,
                SelectionByTicket = ticketData,
                SelectionByTurnover = turnoverData
            };

            return result;
        }

        /// <summary>
        /// Gets summary data from memory cache.
        /// </summary>
        /// <param name="bettype">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartSummaryCache object contains the summary data.</returns>
        public ChartSummaryCache GetSummaryCache(int bettype, DateTime winlostDate)
        {
            string key = TopSelection + bettype;
            var result = CacheServiceInstance.Get<ChartSummaryCache>(key);
            if (result == null)
            {
                result = GetSummary(bettype, winlostDate);
                CacheServiceInstance.Set<ChartSummaryCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }

        /// <summary>
        /// Queries top leagues and top currencies by ticket and saves them to memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetTopSelectionByTicketCache(int selection, int sportType, DateTime winlostDate)
        {
            // check cache
            var result = GetTopSelectionByTicketCache(selection, sportType, winlostDate);

            string key = ByTicket + selection + sportType;
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        result = GetTopSelectionByTicket(selection, sportType, winlostDate);
                        CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                    }
                }
            }
        }

        /// <summary>
        /// Gets top leagues and top currencies by ticket.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains queried top leagues and top currencies.</returns>
        public ChartDetailCache GetTopSelectionByTicket(int selection, int sportType, DateTime winlostDate)
        {
            var leagueData = ChartsServiceInstance.GetTopLeagueByTicket(selection, sportType, winlostDate, maxTransId);
            var currencyData = ChartsServiceInstance.GetTopCurrencyByTicket(selection, sportType, winlostDate, maxTransId);
            var result = new ChartDetailCache
            {
                MaxTransId = maxTransId,
                LastUpdateTime = DateTime.Now,
                SelectionByTicket_League = leagueData,
                SelectionbyTicket_Currency = currencyData
            };

            return result;
        }

        /// <summary>
        /// Gets top leagues and top currencies by ticket from memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains queried top leagues and top currencies.</returns>
        public ChartDetailCache GetTopSelectionByTicketCache(int selection, int sportType, DateTime winlostDate)
        {
            string key = ByTicket + selection + sportType;
            var result = CacheServiceInstance.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = GetTopSelectionByTicket(selection, sportType, winlostDate);
                CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }

        /// <summary>
        /// Queries top leagues and top currencies by turnover from DB and saves them to the memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetTopSelectionByTurnoverCache(int selection, int sportType, DateTime winlostDate)
        {
            // check cache
            string key = ByTurnover + selection + sportType;
            var result = GetTopSelectionByTurnoverCache(selection, sportType, winlostDate);

            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        result = GetTopSelectionByTurnover(selection, sportType, winlostDate);
                        CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                    }
                }
            }
        }

        /// <summary>
        /// Gets top leagues and top currencies by turnover.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains top leagues and top currencies data.</returns>
        public ChartDetailCache GetTopSelectionByTurnover(int selection, int sportType, DateTime winlostDate)
        {
            var leagueData = ChartsServiceInstance.GetTopLeagueByTurnover(selection, sportType, winlostDate, maxTransId);
            var currencyData = ChartsServiceInstance.GetTopCurrencyByTurnover(selection, sportType, winlostDate, maxTransId);
            var result = new ChartDetailCache
            {
                MaxTransId = maxTransId,
                LastUpdateTime = DateTime.Now,
                SelectionByTurnover_League = leagueData,
                SelectionbyTurnover_Currency = currencyData
            };

            return result;
        }

        /// <summary>
        /// Gets top leagues and top currencies by turnover from memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains top leagues and top currencies data.</returns>
        public ChartDetailCache GetTopSelectionByTurnoverCache(int selection, int sportType, DateTime winlostDate)
        {
            string key = ByTurnover + selection + sportType;
            var result = CacheServiceInstance.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = GetTopSelectionByTurnover(selection, sportType, winlostDate);
                CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }

        /// <summary>
        /// Get Top Selection By Ticket Event By Currency.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="currency">Currency ID.</param>
        /// <param name="topCurrency">Top Currencies.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>Cached details of chart.</returns>
        public ChartDetailCache GetTopSelectionByTicketEventByCurrency(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate)
        {
            var data = ChartsServiceInstance.GetTopSelectionTicketEventByCurrency(selection, sportType, currency, topCurrency, winlostDate, maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = maxTransId,
                SelectionbyTicketEventByCurrency = data
            };

            return result;
        }

        /// <summary>
        /// Get Top Selection By Ticket Event By Currency Cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="currency">Currency ID.</param>
        /// <param name="topCurrency">Top Currencies.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>Cached details of chart.</returns>
        public ChartDetailCache GetTopSelectionByTicketEventByCurrencyCache(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate)
        {
            string key = ByTicket + ByCurrency + selection + sportType + currency;
            var result = CacheServiceInstance.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = GetTopSelectionByTicketEventByCurrency(selection, sportType, currency, topCurrency, winlostDate);
                CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }

        /// <summary>
        /// Gets top events by top turnovers and currency.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="currency">Currency ID.</param>
        /// <param name="topCurrency">Top currency.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains top events data.</returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventByCurrency(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate)
        {
            var data = ChartsServiceInstance.GetTopSelectionTurnoverEventByCurrency(selection, sportType, currency, topCurrency, winlostDate, maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = maxTransId,
                SelectionbyTurnoverEventByCurrency = data
            };

            return result;
        }

        /// <summary>
        /// Get Top Selection By Turnover Event By Currency Cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="currency">Currency ID.</param>
        /// <param name="topCurrency">Top Currencies.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>Cached details of chart.</returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventByCurrencyCache(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate)
        {
            string key = ByTurnover + ByCurrency + selection + sportType + currency;
            var result = CacheServiceInstance.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = GetTopSelectionByTurnoverEventByCurrency(selection, sportType, currency, topCurrency, winlostDate);
                CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }

        /// <summary>
        /// Gets top events by top tickets and league.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache objects that contains top events data.</returns>
        public ChartDetailCache GetTopSelectionByTicketEventByLeague(int selection, int sportType, int leagueId, DateTime winlostDate)
        {
            var data = ChartsServiceInstance.GetTopSelectionTicketEventByLeague(selection, sportType, leagueId, winlostDate, maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = maxTransId,
                SelectionbyTicketEventByLeague = data
            };

            return result;
        }

        /// <summary>
        /// Gets top events by top tickets and league from memory cache.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache objects that contains top events data.</returns>
        public ChartDetailCache GetTopSelectionByTicketEventByLeagueCache(int selection, int sportType, int leagueId, DateTime winlostDate)
        {
            string key = ByTicket + ByLeague + selection + sportType + leagueId;
            var result = CacheServiceInstance.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = GetTopSelectionByTicketEventByLeague(selection, sportType, leagueId, winlostDate);
                CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }

        /// <summary>
        /// Gets top events by top tickets and currency.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains top events data.</returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventByLeague(int selection, int sportType, int leagueId, DateTime winlostDate)
        {
            var data = ChartsServiceInstance.GetTopSelectionTurnoverEventByLeague(selection, sportType, leagueId, winlostDate, maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = maxTransId,
                SelectionbyTurnoverEventByLeague = data
            };

            return result;
        }

        /// <summary>
        /// Gets top events by top tickets and currency from memory cache.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains top events data.</returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventByLeagueCache(int selection, int sportType, int leagueId, DateTime winlostDate)
        {
            string key = ByTurnover + ByLeague + selection + sportType + leagueId;
            var result = CacheServiceInstance.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = GetTopSelectionByTurnoverEventByLeague(selection, sportType, leagueId, winlostDate);
                CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }

        /// <summary>
        /// Set Top Selections of Ticket and Event By Currency.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="currency">Currency ID.</param>
        /// <param name="topCurrency">Top Currencies.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetTopSelectionByTicketEventByCurrencyCache(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate)
        {
            // check cache
            string key = ByTicket + ByCurrency + selection + sportType + currency;
            var result = GetTopSelectionByTicketEventByCurrencyCache(selection, sportType, currency, topCurrency, winlostDate);

            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        result = GetTopSelectionByTicketEventByCurrency(selection, sportType, currency, topCurrency, winlostDate);
                        CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                    }
                }
            }
        }

        /// <summary>
        /// Set Top Selections of Turnover Event By Currency.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="currency">Currency ID.</param>
        /// <param name="topCurrency">Top Currencies.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetTopSelectionByTurnoverEventByCurrencyCache(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate)
        {
            // check cache
            string key = ByTurnover + ByCurrency + selection + sportType + currency;
            var result = GetTopSelectionByTurnoverEventByCurrencyCache(selection, sportType, currency, topCurrency, winlostDate);

            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        result = GetTopSelectionByTurnoverEventByCurrency(selection, sportType, currency, topCurrency, winlostDate);
                        CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                    }
                }
            }
        }

        /// <summary>
        /// Queries top events by top tickets and league from DB and saves them to memory cache.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetTopSelectionByTicketEventByLeagueCache(int selection, int sportType, int leagueId, DateTime winlostDate)
        {
            // check cache
            string key = ByTicket + ByLeague + selection + sportType + leagueId;
            var result = GetTopSelectionByTicketEventByLeagueCache(selection, sportType, leagueId, winlostDate);

            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        result = GetTopSelectionByTicketEventByLeague(selection, sportType, leagueId, winlostDate);
                        CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                    }
                }
            }
        }

        /// <summary>
        /// Queries top events by top tickets and currency from DB and save them to cache.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="leagueId">League ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetTopSelectionByTurnoverEventByLeagueCache(int selection, int sportType, int leagueId, DateTime winlostDate)
        {
            // check cache
            string key = ByTurnover + ByLeague + selection + sportType + leagueId;
            var result = GetTopSelectionByTurnoverEventByLeagueCache(selection, sportType, leagueId, winlostDate);

            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        result = GetTopSelectionByTurnoverEventByLeague(selection, sportType, leagueId, winlostDate);
                        CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                    }
                }
            }
        }

        /// <summary>
        /// Queries top bet types by event from DB and saves them to memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="matchId">The Match ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetTopSelectionByTicketEventBettypeCache(int selection, int sportType, int matchId, DateTime winlostDate)
        {
            string key = ByTicket + selection + sportType + matchId;
            var result = GetTopSelectionByTicketEventBettypeCache(selection, sportType, matchId, winlostDate);
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        result = this.GetTopSelectionByTicketEventBettype(selection, sportType, matchId, winlostDate);
                        CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                    }
                }
            }
        }

        /// <summary>
        /// Gets top bet types by event.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <returns>ChartDetailCache object contains top bet types.</returns>
        public ChartDetailCache GetTopSelectionByTicketEventBettype(int selection, int sportType, int matchId, DateTime winlostDate)
        {
            var data = ChartsServiceInstance.GetTopSelectionByTicket_Event_Bettype(selection, sportType, matchId, winlostDate, maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = maxTransId,
                SelectionByTicketEventBettype = data
            };

            return result;
        }

        /// <summary>
        /// Gets top bet types by event from memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">The match ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains top bet types.</returns>
        public ChartDetailCache GetTopSelectionByTicketEventBettypeCache(int selection, int sportType, int matchId, DateTime winlostDate)
        {
            string key = ByTicket + selection + sportType + matchId;
            var result = CacheServiceInstance.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = this.GetTopSelectionByTicketEventBettype(selection, sportType, matchId, winlostDate);
                CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }

        /// <summary>
        /// Queries top bet types by event from DB and saves them to memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="matchId">The Match ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetTopSelectionByTurnoverBettypeCache(int selection, int sportType, int matchId, DateTime winlostDate)
        {
            string key = ByTurnover + selection + sportType + matchId;
            var result = GetTopSelectionByTuroverBettypeCache(selection, sportType, matchId, winlostDate);
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        result = this.GetTopSelectionByTuroverBettype(selection, sportType, matchId, winlostDate);
                        CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                    }
                }
            }
        }

        /// <summary>
        /// Gets top bet types by event.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <returns>ChartDetailCache object contains top bet types.</returns>
        public ChartDetailCache GetTopSelectionByTuroverBettype(int selection, int sportType, int matchId, DateTime winlostDate)
        {
            var data = ChartsServiceInstance.GetTopSelectionByTurnover_Event_Bettype(selection, sportType, matchId, winlostDate, maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = maxTransId,
                SelectionByTurnoverEventBettype = data
            };

            return result;
        }

        /// <summary>
        /// Gets top bet types by event from memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="matchId">The Match ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains top bet types.</returns>
        public ChartDetailCache GetTopSelectionByTuroverBettypeCache(int selection, int sportType, int matchId, DateTime winlostDate)
        {
            string key = ByTurnover + selection + sportType + matchId;
            var result = CacheServiceInstance.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = this.GetTopSelectionByTuroverBettype(selection, sportType, matchId, winlostDate);
                CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }

        /// <summary>
        /// Queries report favorite choices by event and bet type from DB and saves them to memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">The match ID.</param>
        /// <param name="betType2">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetTopSelectionByTicketEventBettypeChoiceCache(int selection, int sportType, int matchId, int betType2, DateTime winlostDate)
        {
            string key = ByTicket + selection + sportType + matchId + betType2;
            var result = this.GetTopSelectionByTicketEventBettypeChoiceCache(selection, sportType, matchId, betType2, winlostDate);
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        result = this.GetTopSelectionByTicketEventBettypeChoice(selection, sportType, matchId, betType2, winlostDate);
                        CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                    }
                }
            }
        }

        /// <summary>
        /// Gets report favorite choices by event and bet type.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="betType2">Bet type ID.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <returns>ChartDetailCache object contains favorite choices.</returns>
        public ChartDetailCache GetTopSelectionByTicketEventBettypeChoice(int selection, int sportType, int matchId, int betType2, DateTime winlostDate)
        {
            var data = ChartsServiceInstance.GetTopSelectionByTicket_Event_Bettype_Choice(selection, sportType, matchId, betType2, winlostDate, maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = maxTransId,
                SelectionByTicketEventBettypeChoice = data
            };

            return result;
        }

        /// <summary>
        /// Gets report favorite choices by event and bet type from memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">The match ID.</param>
        /// <param name="betType2">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains favorite choices.</returns>
        public ChartDetailCache GetTopSelectionByTicketEventBettypeChoiceCache(int selection, int sportType, int matchId, int betType2, DateTime winlostDate)
        {
            string key = ByTicket + selection + sportType + matchId + betType2;
            var result = CacheServiceInstance.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = this.GetTopSelectionByTicketEventBettypeChoice(selection, sportType, matchId, betType2, winlostDate);
                CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }

        /// <summary>
        /// Queries report favorite choices by event and bet type from DB and saves them to memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="matchId">The Match ID.</param>
        /// <param name="betType2">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetTopSelectionByTurnoverEventBettypeChoiceCache(int selection, int sportType, int matchId, int betType2, DateTime winlostDate)
        {
            string key = ByTurnover + selection + sportType + matchId + betType2;
            var result = this.GetTopSelectionByTurnoverEventBettypeChoiceCache(selection, sportType, matchId, betType2, winlostDate);
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        result = this.GetTopSelectionByTurnoverEventBettypeChoice(selection, sportType, matchId, betType2, winlostDate);
                        CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                    }
                }
            }
        }

        /// <summary>
        /// Gets report favorite choices by event and bet type.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="betType2">Bet type ID.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <returns>ChartDetailCache object contains favorite choices.</returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventBettypeChoice(int selection, int sportType, int matchId, int betType2, DateTime winlostDate)
        {
            var data = ChartsServiceInstance.GetTopSelectionByTurnover_Event_Bettype_Choice(selection, sportType, matchId, betType2, winlostDate, maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = maxTransId,
                SelectionByTurnoverEventBettypeChoice = data
            };

            return result;
        }

        /// <summary>
        /// Gets report favorite choices by event and bet type from memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="matchId">The Match ID.</param>
        /// <param name="betType2">Bet type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains favorite choices.</returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventBettypeChoiceCache(int selection, int sportType, int matchId, int betType2, DateTime winlostDate)
        {
            string key = ByTurnover + selection + sportType + matchId + betType2;
            var result = CacheServiceInstance.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = this.GetTopSelectionByTurnoverEventBettypeChoice(selection, sportType, matchId, betType2, winlostDate);
                CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }

        /// <summary>
        /// Queries top events by top selections of ticket from DB and saves them to memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetTopSelectionByTicketEventCache(int selection, int sportType, DateTime winlostDate)
        {
            string key = ByTicket + ByEvent + selection + sportType;
            var result = this.GetTopSelectionByTicketEventCache(selection, sportType, winlostDate);
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        result = this.GetTopSelectionByTicketEvent(selection, sportType, winlostDate);
                        CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                    }
                }
            }
        }

        /// <summary>
        /// Gets top events by top selections of ticket.
        /// </summary>
        /// <param name="selection">Selection value.</param>
        /// <param name="sportType">Sport type ID.</param>
        /// <param name="winlostDate">Win Loss Date.</param>
        /// <returns>ChartDetailCache object contains top events data.</returns>
        public ChartDetailCache GetTopSelectionByTicketEvent(int selection, int sportType, DateTime winlostDate)
        {
            var data = ChartsServiceInstance.GetTopSelectionByTicket_Event(selection, sportType, winlostDate, maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = maxTransId,
                SelectionByTicketEvent = data
            };

            return result;
        }

        /// <summary>
        /// Gets top events by top selections of ticket from memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains top events data.</returns>
        public ChartDetailCache GetTopSelectionByTicketEventCache(int selection, int sportType, DateTime winlostDate)
        {
            string key = ByTicket + ByEvent + selection + sportType;
            var result = CacheServiceInstance.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = this.GetTopSelectionByTicketEvent(selection, sportType, winlostDate);
                CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }

        /// <summary>
        /// Queries top events by top selection of turnover from DB and saves them to the memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        public void SetTopSelectionByTurnoverEventCache(int selection, int sportType, DateTime winlostDate)
        {
            string key = ByTurnover + ByEvent + selection + sportType;
            var result = this.GetTopSelectionByTurnoverEventCache(selection, sportType, winlostDate);
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(RefreshCache))
                    {
                        result = this.GetTopSelectionByTurnoverEvent(selection, sportType, winlostDate);
                        CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
                    }
                }
            }
        }

        /// <summary>
        /// Gets top events by top selection of turnover.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains top events data.</returns>
        public ChartDetailCache GetTopSelectionByTurnoverEvent(int selection, int sportType, DateTime winlostDate)
        {
            var data = ChartsServiceInstance.GetTopSelectionByTurnover_Event(selection, sportType, winlostDate, maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = maxTransId,
                SelectionByTurnoverEvent = data
            };

            return result;
        }

        /// <summary>
        /// Gets top events by top selection of turnover from memory cache.
        /// </summary>
        /// <param name="selection">Selection ID.</param>
        /// <param name="sportType">Sport Type ID.</param>
        /// <param name="winlostDate">Win loss date.</param>
        /// <returns>ChartDetailCache object contains top events data.</returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventCache(int selection, int sportType, DateTime winlostDate)
        {
            string key = ByTurnover + ByEvent + selection + sportType;
            var result = CacheServiceInstance.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = this.GetTopSelectionByTurnoverEvent(selection, sportType, winlostDate);
                CacheServiceInstance.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(ExpiredCache));
            }

            return result;
        }
    }
}