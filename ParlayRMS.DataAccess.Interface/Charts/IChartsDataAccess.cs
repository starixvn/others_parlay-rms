namespace ParlayRMS.DataAccess.Interface.Charts
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.Charts;

    public interface IChartsDataAccess
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_Sum> RMS_Monitor_Parlay_Sum(int bettype, DateTime winlostDate, out long maxTransId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTicket> RMS_Monitor_Parlay_SelectionByTicket(DateTime winlostDate, long maxTransId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover> RMS_Monitor_Parlay_SelectionByTurnover(DateTime winlostDate, long maxTransId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League> RMS_Monitor_Parlay_SelectionByTicket_League(int selection, int sportType, DateTime winlostDate, long maxTransId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League> RMS_Monitor_Parlay_SelectionByTurnover_League(int selection, int sportType, DateTime winlostDate, long maxTransId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency> RMS_Monitor_Parlay_SelectionByTicket_Currency(int selection, int sportType, DateTime winlostDate, long maxTransId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionbyTurnover_Currency> RMS_Monitor_Parlay_SelectionByTurnover_Currency(int selection, int sportType, DateTime winlostDate, long maxTransId);

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
        IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event> RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate, long maxTransId);

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
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event> RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate, long maxTransId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League_Event> RMS_Monitor_Parlay_SelectionByTicket_League_Event(int selection, int sportType, int leagueId, DateTime winlostDate, long maxTransId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League_Event> RMS_Monitor_Parlay_SelectionByTurnover_League_Event(int selection, int sportType, int leagueId, DateTime winlostDate, long maxTransId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event> RMS_Monitor_Parlay_SelectionByTurnover_Event(int selection, int sportType, DateTime winlostDate, long maxTransId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype> RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype(int selection, int sportType, int matchId, DateTime winlostDate, long maxTransId);

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
        IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice> RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice(int selection, int sportType, int matchId, int betType2, DateTime winlostDate, long maxTransId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event> RMS_Monitor_Parlay_SelectionByTicket_Event(int selection, int sportType, DateTime winlostDate, long maxTransId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype> RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype(int selection, int sportType, int matchId, DateTime winlostDate, long maxTransId);

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
        IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice> RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice(int selection, int sportType, int matchId, int betType2, DateTime winlostDate, long maxTransId);
    }
}