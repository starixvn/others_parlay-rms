namespace ParlayRMS.Services.Implement.Charts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Caching;
    using ParlayRMS.DTO;
    using ParlayRMS.Models;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Web.Session;
    using ParlayRMS.DTO.Charts;
    using ParlayRMS.Services.Interface.Charts;
    using ParlayRMS.Models.Charts;

    public class ChartsCacheService : BaseService, IChartsService
    {
        private const int REFRESHCACHE = 10; // 10s
        private const string BYTICKET = "BYTICKET";
        private const string BYTURNOVER = "BYTURNOVER";
        private const string TOPSELECTION = "TOPSELECTION";
        private const string BYCURRENCY = "BYCURRENCY";
        private const string BYLEAGUE = "BYLEAGUE";
        private const string BYEVENT = "BYEVENT";

        private const int EXPIREDCACHE = 60; // 60s

        private static readonly ICache cacheService = CacheService.CreateInstance(CacheType.MemoryCache);
        private static readonly IChartsService chartsService = new ChartsService();
        private static readonly ICurrentSession currentSession = new CurrentSession();

        private static object locker = new object();

        private static DateTime lastTimeSummary = DateTime.Now;
        private static long s_maxTransId = 0;
        public static readonly bool IsCache = Utility.AppSetting.IsCache;

        #region SummaryData

        /// <summary>
        ///
        /// </summary>
        /// <param name="bettype"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_Sum> GetSumDataResult(int bettype, DateTime winlostDate, out long maxTransId)
        {
            maxTransId = ChartsCacheService.MaxTransId;

            if (!ChartsCacheService.IsCache)
            {
                var result = chartsService.GetSumDataResult(bettype, winlostDate, out maxTransId);
                ChartsCacheService.MaxTransId = maxTransId;
                return result;
            }

            return this.GetSummaryCache(bettype, winlostDate).Sum;
        }

        #endregion SummaryData

        #region ByTicket

        /// <summary>
        ///
        /// </summary>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket> GetTopSelectionByTicket(DateTime winlostDate, long maxTransId)
        {
            return this.GetSummaryCache(9, winlostDate).SelectionByTicket;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League> GetTopLeagueByTicket(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTicketCache(selection, sportType, winlostDate).SelectionByTicket_League;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency> GetTopCurrencyByTicket(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTicketCache(selection, sportType, winlostDate).SelectionbyTicket_Currency;
        }

        #endregion ByTicket

        #region ByTurnover

        /// <summary>
        ///
        /// </summary>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover> GetTopSelectionByTurnover(DateTime winlostDate, long maxTransId)
        {
            return this.GetSummaryCache(9, winlostDate).SelectionByTurnover;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League> GetTopLeagueByTurnover(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTurnoverCache(selection, sportType, winlostDate).SelectionByTurnover_League;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTurnover_Currency> GetTopCurrencyByTurnover(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTurnoverCache(selection, sportType, winlostDate).SelectionbyTurnover_Currency;
        }

        #endregion ByTurnover

        #region ByCurrency

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="currency"></param>
        /// <param name="topCurrency"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event> GetTopSelectionTicketEventByCurrency(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTicketEventByCurrencyCache(selection, sportType, currency, topCurrency, winlostDate).SelectionbyTicketEventByCurrency;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="currency"></param>
        /// <param name="topCurrency"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event> GetTopSelectionTurnoverEventByCurrency(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTurnoverEventByCurrencyCache(selection, sportType, currency, topCurrency, winlostDate).SelectionbyTurnoverEventByCurrency;
        }

        #endregion ByCurrency

        #region ByLeague

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League_Event> GetTopSelectionTicketEventByLeague(int selection, int sportType, int leagueId, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTicketEventByLeagueCache(selection, sportType, leagueId, winlostDate).SelectionbyTicketEventByLeague;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League_Event> GetTopSelectionTurnoverEventByLeague(int selection, int sportType, int leagueId, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTurnoverEventByLeagueCache(selection, sportType, leagueId, winlostDate).SelectionbyTurnoverEventByLeague;
        }

        #endregion ByLeague

        #region Event

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event> GetTopSelectionByTurnover_Event(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTurnoverEventCache(selection, sportType, winlostDate).SelectionByTurnoverEvent;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype> GetTopSelectionByTurnover_Event_Bettype(int selection, int sportType, int matchId, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTuroverBettypeCache(selection, sportType, matchId, winlostDate).SelectionByTurnoverEventBettype;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="betType2"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice> GetTopSelectionByTurnover_Event_Bettype_Choice(int selection, int sportType, int matchId, int betType2, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTurnoverEventBettypeChoiceCache(selection, sportType, matchId, betType2, winlostDate).SelectionByTurnoverEventBettypeChoice;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event> GetTopSelectionByTicket_Event(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTicketEventCache(selection, sportType, winlostDate).SelectionByTicketEvent;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype> GetTopSelectionByTicket_Event_Bettype(int selection, int sportType, int matchId, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTicketEventBettypeCache(selection, sportType, matchId, winlostDate).SelectionByTicketEventBettype;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="betType2"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice> GetTopSelectionByTicket_Event_Bettype_Choice(int selection, int sportType, int matchId, int betType2, DateTime winlostDate, long maxTransId)
        {
            return this.GetTopSelectionByTicketEventBettypeChoiceCache(selection, sportType, matchId, betType2, winlostDate).SelectionByTicketEventBettypeChoice;
        }

        #endregion Event

        public static long MaxTransId
        {
            get
            {
                return s_maxTransId;
            }
            set
            {
                s_maxTransId = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
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

        public void SetCache(int bettype, DateTime winlostDate, bool isTicket, int selection, int sportType, int currency, string topCurrency, int leagueId)
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
        ///
        /// </summary>
        public void SetSummaryCache(int bettype, DateTime winlostDate)
        {
            if (!IsCache)
            {
                return;
            }

            string key = TOPSELECTION + bettype;

            if (DateTime.Now.Subtract(lastTimeSummary) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(lastTimeSummary) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        var result = GetSummary(bettype, winlostDate);
                        cacheService.Set<ChartSummaryCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                        lastTimeSummary = DateTime.Now;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="currency"></param>
        /// <param name="topCurrency"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="isTicket"></param>
        public void SetCacheEvent(int selection, int sportType, int currency, string topCurrency, int leagueId, DateTime winlostDate, bool isTicket)
        {
            if (!IsCache)
            {
                return;
            }

            if (currency != -1) // by Currency
            {
                if (isTicket) // by Ticket
                {
                    this.SetTopSelectionByTicketEventByCurrencyCache(selection, sportType, currency, topCurrency, winlostDate);
                }
                else // by Turover
                {
                    this.SetTopSelectionByTurnoverEventByCurrencyCache(selection, sportType, currency, topCurrency, winlostDate);
                }
            }
            else // by League
            {
                if (isTicket) // by Ticket
                {
                    this.SetTopSelectionByTicketEventByLeagueCache(selection, sportType, leagueId, winlostDate);
                }
                else // by Turover
                {
                    this.SetTopSelectionByTurnoverEventByLeagueCache(selection, sportType, leagueId, winlostDate);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="isTicket"></param>
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
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="betType2"></param>
        /// <param name="winlostDate"></param>
        /// <param name="isTicket"></param>
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
        ///
        /// </summary>
        /// <param name="bettype"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartSummaryCache GetSummary(int bettype, DateTime winlostDate)
        {
            long newMaxTransId = 0;
            var sumData = chartsService.GetSumDataResult(bettype, winlostDate, out newMaxTransId);
            s_maxTransId = newMaxTransId;
            var ticketData = chartsService.GetTopSelectionByTicket(winlostDate, s_maxTransId);
            var turnoverData = chartsService.GetTopSelectionByTurnover(winlostDate, s_maxTransId);
            var result = new ChartSummaryCache
            {
                Sum = sumData,
                SelectionByTicket = ticketData,
                SelectionByTurnover = turnoverData
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartSummaryCache GetSummaryCache(int bettype, DateTime winlostDate)
        {
            string key = TOPSELECTION + bettype;
            var result = cacheService.Get<ChartSummaryCache>(key);
            if (result == null)
            {
                result = GetSummary(bettype, winlostDate);
                cacheService.Set<ChartSummaryCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        public void SetTopSelectionByTicketCache(int selection, int sportType, DateTime winlostDate)
        {
            // check cache
            var result = GetTopSelectionByTicketCache(selection, sportType, winlostDate);

            string key = BYTICKET + selection + sportType;
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        result = GetTopSelectionByTicket(selection, sportType, winlostDate);
                        cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTicket(int selection, int sportType, DateTime winlostDate)
        {
            var leagueData = chartsService.GetTopLeagueByTicket(selection, sportType, winlostDate, s_maxTransId);
            var currencyData = chartsService.GetTopCurrencyByTicket(selection, sportType, winlostDate, s_maxTransId);
            var result = new ChartDetailCache
            {
                MaxTransId = s_maxTransId,
                LastUpdateTime = DateTime.Now,
                SelectionByTicket_League = leagueData,
                SelectionbyTicket_Currency = currencyData
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTicketCache(int selection, int sportType, DateTime winlostDate)
        {
            string key = BYTICKET + selection + sportType;
            var result = cacheService.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = GetTopSelectionByTicket(selection, sportType, winlostDate);
                cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        public void SetTopSelectionByTurnoverCache(int selection, int sportType, DateTime winlostDate)
        {
            // check cache
            string key = BYTURNOVER + selection + sportType;
            var result = GetTopSelectionByTurnoverCache(selection, sportType, winlostDate);

            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        result = GetTopSelectionByTurnover(selection, sportType, winlostDate);
                        cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTurnover(int selection, int sportType, DateTime winlostDate)
        {
            var leagueData = chartsService.GetTopLeagueByTurnover(selection, sportType, winlostDate, s_maxTransId);
            var currencyData = chartsService.GetTopCurrencyByTurnover(selection, sportType, winlostDate, s_maxTransId);
            var result = new ChartDetailCache
            {
                MaxTransId = s_maxTransId,
                LastUpdateTime = DateTime.Now,
                SelectionByTurnover_League = leagueData,
                SelectionbyTurnover_Currency = currencyData
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTurnoverCache(int selection, int sportType, DateTime winlostDate)
        {
            string key = BYTURNOVER + selection + sportType;
            var result = cacheService.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = GetTopSelectionByTurnover(selection, sportType, winlostDate);
                cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="currency"></param>
        /// <param name="topCurrency"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTicketEventByCurrency(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate)
        {
            var data = chartsService.GetTopSelectionTicketEventByCurrency(selection, sportType, currency, topCurrency, winlostDate, s_maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = s_maxTransId,
                SelectionbyTicketEventByCurrency = data
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="currency"></param>
        /// <param name="topCurrency"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTicketEventByCurrencyCache(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate)
        {
            string key = BYTICKET + BYCURRENCY + selection + sportType + currency;
            var result = cacheService.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = GetTopSelectionByTicketEventByCurrency(selection, sportType, currency, topCurrency, winlostDate);
                cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="currency"></param>
        /// <param name="topCurrency"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventByCurrency(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate)
        {
            var data = chartsService.GetTopSelectionTurnoverEventByCurrency(selection, sportType, currency, topCurrency, winlostDate, s_maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = s_maxTransId,
                SelectionbyTurnoverEventByCurrency = data
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="currency"></param>
        /// <param name="topCurrency"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventByCurrencyCache(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate)
        {
            string key = BYTURNOVER + BYCURRENCY + selection + sportType + currency;
            var result = cacheService.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = GetTopSelectionByTurnoverEventByCurrency(selection, sportType, currency, topCurrency, winlostDate);
                cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTicketEventByLeague(int selection, int sportType, int leagueId, DateTime winlostDate)
        {
            var data = chartsService.GetTopSelectionTicketEventByLeague(selection, sportType, leagueId, winlostDate, s_maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = s_maxTransId,
                SelectionbyTicketEventByLeague = data
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTicketEventByLeagueCache(int selection, int sportType, int leagueId, DateTime winlostDate)
        {
            string key = BYTICKET + BYLEAGUE + selection + sportType + leagueId;
            var result = cacheService.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = GetTopSelectionByTicketEventByLeague(selection, sportType, leagueId, winlostDate);
                cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventByLeague(int selection, int sportType, int leagueId, DateTime winlostDate)
        {
            var data = chartsService.GetTopSelectionTurnoverEventByLeague(selection, sportType, leagueId, winlostDate, s_maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = s_maxTransId,
                SelectionbyTurnoverEventByLeague = data
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventByLeagueCache(int selection, int sportType, int leagueId, DateTime winlostDate)
        {
            string key = BYTURNOVER + BYLEAGUE + selection + sportType + leagueId;
            var result = cacheService.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = GetTopSelectionByTurnoverEventByLeague(selection, sportType, leagueId, winlostDate);
                cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="currency"></param>
        /// <param name="topCurrency"></param>
        /// <param name="winlostDate"></param>
        public void SetTopSelectionByTicketEventByCurrencyCache(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate)
        {
            // check cache
            string key = BYTICKET + BYCURRENCY + selection + sportType + currency;
            var result = GetTopSelectionByTicketEventByCurrencyCache(selection, sportType, currency, topCurrency, winlostDate);

            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        result = GetTopSelectionByTicketEventByCurrency(selection, sportType, currency, topCurrency, winlostDate);
                        cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="currency"></param>
        /// <param name="topCurrency"></param>
        /// <param name="winlostDate"></param>
        public void SetTopSelectionByTurnoverEventByCurrencyCache(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate)
        {
            // check cache
            string key = BYTURNOVER + BYCURRENCY + selection + sportType + currency;
            var result = GetTopSelectionByTurnoverEventByCurrencyCache(selection, sportType, currency, topCurrency, winlostDate);

            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        result = GetTopSelectionByTurnoverEventByCurrency(selection, sportType, currency, topCurrency, winlostDate);
                        cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        public void SetTopSelectionByTicketEventByLeagueCache(int selection, int sportType, int leagueId, DateTime winlostDate)
        {
            // check cache
            string key = BYTICKET + BYLEAGUE + selection + sportType + leagueId;
            var result = GetTopSelectionByTicketEventByLeagueCache(selection, sportType, leagueId, winlostDate);

            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        result = GetTopSelectionByTicketEventByLeague(selection, sportType, leagueId, winlostDate);
                        cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        public void SetTopSelectionByTurnoverEventByLeagueCache(int selection, int sportType, int leagueId, DateTime winlostDate)
        {
            // check cache
            string key = BYTURNOVER + BYLEAGUE + selection + sportType + leagueId;
            var result = GetTopSelectionByTurnoverEventByLeagueCache(selection, sportType, leagueId, winlostDate);

            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        result = GetTopSelectionByTurnoverEventByLeague(selection, sportType, leagueId, winlostDate);
                        cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        public void SetTopSelectionByTicketEventBettypeCache(int selection, int sportType, int matchId, DateTime winlostDate)
        {
            string key = BYTICKET + selection + sportType + matchId;
            var result = GetTopSelectionByTicketEventBettypeCache(selection, sportType, matchId, winlostDate);
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        result = this.GetTopSelectionByTicketEventBettype(selection, sportType, matchId, winlostDate);
                        cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTicketEventBettype(int selection, int sportType, int matchId, DateTime winlostDate)
        {
            var data = chartsService.GetTopSelectionByTicket_Event_Bettype(selection, sportType, matchId, winlostDate, s_maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = s_maxTransId,
                SelectionByTicketEventBettype = data
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTicketEventBettypeCache(int selection, int sportType, int matchId, DateTime winlostDate)
        {
            string key = BYTICKET + selection + sportType + matchId;
            var result = cacheService.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = this.GetTopSelectionByTicketEventBettype(selection, sportType, matchId, winlostDate);
                cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        public void SetTopSelectionByTurnoverBettypeCache(int selection, int sportType, int matchId, DateTime winlostDate)
        {
            string key = BYTURNOVER + selection + sportType + matchId;
            var result = GetTopSelectionByTuroverBettypeCache(selection, sportType, matchId, winlostDate);
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        result = this.GetTopSelectionByTuroverBettype(selection, sportType, matchId, winlostDate);
                        cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTuroverBettype(int selection, int sportType, int matchId, DateTime winlostDate)
        {
            var data = chartsService.GetTopSelectionByTurnover_Event_Bettype(selection, sportType, matchId, winlostDate, s_maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = s_maxTransId,
                SelectionByTurnoverEventBettype = data
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTuroverBettypeCache(int selection, int sportType, int matchId, DateTime winlostDate)
        {
            string key = BYTURNOVER + selection + sportType + matchId;
            var result = cacheService.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = this.GetTopSelectionByTuroverBettype(selection, sportType, matchId, winlostDate);
                cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="betType2"></param>
        /// <param name="winlostDate"></param>
        public void SetTopSelectionByTicketEventBettypeChoiceCache(int selection, int sportType, int matchId, int betType2, DateTime winlostDate)
        {
            string key = BYTICKET + selection + sportType + matchId + betType2;
            var result = this.GetTopSelectionByTicketEventBettypeChoiceCache(selection, sportType, matchId, betType2, winlostDate);
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        result = this.GetTopSelectionByTicketEventBettypeChoice(selection, sportType, matchId, betType2, winlostDate);
                        cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="betType2"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTicketEventBettypeChoice(int selection, int sportType, int matchId, int betType2, DateTime winlostDate)
        {
            var data = chartsService.GetTopSelectionByTicket_Event_Bettype_Choice(selection, sportType, matchId, betType2, winlostDate, s_maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = s_maxTransId,
                SelectionByTicketEventBettypeChoice = data
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="betType2"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTicketEventBettypeChoiceCache(int selection, int sportType, int matchId, int betType2, DateTime winlostDate)
        {
            string key = BYTICKET + selection + sportType + matchId + betType2;
            var result = cacheService.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = this.GetTopSelectionByTicketEventBettypeChoice(selection, sportType, matchId, betType2, winlostDate);
                cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="betType2"></param>
        /// <param name="winlostDate"></param>
        public void SetTopSelectionByTurnoverEventBettypeChoiceCache(int selection, int sportType, int matchId, int betType2, DateTime winlostDate)
        {
            string key = BYTURNOVER + selection + sportType + matchId + betType2;
            var result = this.GetTopSelectionByTurnoverEventBettypeChoiceCache(selection, sportType, matchId, betType2, winlostDate);
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        result = this.GetTopSelectionByTurnoverEventBettypeChoice(selection, sportType, matchId, betType2, winlostDate);
                        cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="betType2"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventBettypeChoice(int selection, int sportType, int matchId, int betType2, DateTime winlostDate)
        {
            var data = chartsService.GetTopSelectionByTurnover_Event_Bettype_Choice(selection, sportType, matchId, betType2, winlostDate, s_maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = s_maxTransId,
                SelectionByTurnoverEventBettypeChoice = data
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="betType2"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public ChartDetailCache GetTopSelectionByTurnoverEventBettypeChoiceCache(int selection, int sportType, int matchId, int betType2, DateTime winlostDate)
        {
            string key = BYTURNOVER + selection + sportType + matchId + betType2;
            var result = cacheService.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = this.GetTopSelectionByTurnoverEventBettypeChoice(selection, sportType, matchId, betType2, winlostDate);
                cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }

            return result;
        }

        public void SetTopSelectionByTicketEventCache(int selection, int sportType, DateTime winlostDate)
        {
            string key = BYTICKET + BYEVENT + selection + sportType;
            var result = this.GetTopSelectionByTicketEventCache(selection, sportType, winlostDate);
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        result = this.GetTopSelectionByTicketEvent(selection, sportType, winlostDate);
                        cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                    }
                }
            }
        }

        public ChartDetailCache GetTopSelectionByTicketEvent(int selection, int sportType, DateTime winlostDate)
        {
            var data = chartsService.GetTopSelectionByTicket_Event(selection, sportType, winlostDate, s_maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = s_maxTransId,
                SelectionByTicketEvent = data
            };

            return result;
        }

        public ChartDetailCache GetTopSelectionByTicketEventCache(int selection, int sportType, DateTime winlostDate)
        {
            string key = BYTICKET + BYEVENT + selection + sportType;
            var result = cacheService.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = this.GetTopSelectionByTicketEvent(selection, sportType, winlostDate);
                cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }

            return result;
        }

        public void SetTopSelectionByTurnoverEventCache(int selection, int sportType, DateTime winlostDate)
        {
            string key = BYTURNOVER + BYEVENT + selection + sportType;
            var result = this.GetTopSelectionByTurnoverEventCache(selection, sportType, winlostDate);
            if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
            {
                lock (locker)
                {
                    if (DateTime.Now.Subtract(result.LastUpdateTime) > TimeSpan.FromSeconds(REFRESHCACHE))
                    {
                        result = this.GetTopSelectionByTurnoverEvent(selection, sportType, winlostDate);
                        cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
                    }
                }
            }
        }

        public ChartDetailCache GetTopSelectionByTurnoverEvent(int selection, int sportType, DateTime winlostDate)
        {
            var data = chartsService.GetTopSelectionByTurnover_Event(selection, sportType, winlostDate, s_maxTransId);
            var result = new ChartDetailCache
            {
                LastUpdateTime = DateTime.Now,
                MaxTransId = s_maxTransId,
                SelectionByTurnoverEvent = data
            };

            return result;
        }

        public ChartDetailCache GetTopSelectionByTurnoverEventCache(int selection, int sportType, DateTime winlostDate)
        {
            string key = BYTURNOVER + BYEVENT + selection + sportType;
            var result = cacheService.Get<ChartDetailCache>(key);
            if (result == null)
            {
                result = this.GetTopSelectionByTurnoverEvent(selection, sportType, winlostDate);
                cacheService.Set<ChartDetailCache>(key, result, TimeSpan.FromSeconds(EXPIREDCACHE));
            }

            return result;
        }
    }
}