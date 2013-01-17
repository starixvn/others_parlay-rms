namespace ParlayRMS.Services.Implement.Charts
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DataAccess.Implement;
    using ParlayRMS.DataAccess.Interface;
    using ParlayRMS.DTO;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.DTO.Charts;
    using ParlayRMS.DataAccess.Interface.Charts;
    using ParlayRMS.Services.Interface.Charts;
    using ParlayRMS.DataAccess.Implement.Charts;

    public class ChartsService : BaseService, IChartsService
    {
        private readonly IChartsDataAccess monitorDataAccess = new ChartsDataAccess();

        #region SummaryData

        /// <summary>
        ///
        /// </summary>
        /// <param name="bettype"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_Sum> GetSumDataResult(int bettype, DateTime winlostDate, out long maxTransId)
        {
            return this.monitorDataAccess.RMS_Monitor_Parlay_Sum(bettype, winlostDate, out maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket(winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket_League(selection, sportType, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket_Currency(selection, sportType, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover(winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_League(selection, sportType, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_Currency(selection, sportType, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event(selection, sportType, currency, topCurrency, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event(selection, sportType, currency, topCurrency, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket_League_Event(selection, sportType, leagueId, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_League_Event(selection, sportType, leagueId, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_Event(selection, sportType, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype(selection, sportType, matchId, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice(selection, sportType, matchId, betType2, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket_Event(selection, sportType, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype(selection, sportType, matchId, winlostDate, maxTransId);
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
            return this.monitorDataAccess.RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice(selection, sportType, matchId, betType2, winlostDate, maxTransId);
        }

        #endregion Event
    }
}