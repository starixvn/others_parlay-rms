// --------------------------------------------------------------------------
// <copyright file="CacheController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Controllers
{
    using System;
    using ParlayRMS.Services.Implement.Charts;
    using ParlayRMS.ViewModel.Monitoring;

    /// <summary>
    /// Cache Controller.
    /// </summary>
    public class CacheController : BaseController
    {
        private readonly ChartsCacheService _monitorCacheService = new ChartsCacheService();

        #region Chart

        /// <summary>
        /// Updates top selections in the memory cache.
        /// </summary>
        /// <param name="model">MonitoringCacheModel contains top selections.</param>
        public void SetTopSelection(MonitoringCacheModel model)
        {
            int sportType = GetSportTypeIdByName(model.SportType);
            this._monitorCacheService.SetCache(model.BetType, model.WinlostDate, model.IsTicket, model.Selection, sportType);
            if (model.IsSelectDetail)
            {
                this.SetTopEvent(model);
            }

            if (model.IsEvent)
            {
                this.SetCacheFavouriteChoice(model);
            }
        }

        /// <summary>
        /// Sets the top events to cache.
        /// </summary>
        /// <param name="model">MonitoringCacheModel object contains top events' data.</param>
        public void SetTopEvent(MonitoringCacheModel model)
        {
            int sportType = GetSportTypeIdByName(model.SportType);
            if (model.Currency != -1)
            {
                // by Currency
                if (model.IsTicket)
                {
                    this._monitorCacheService.SetTopSelectionByTicketEventByCurrencyCache(model.Selection, sportType, model.Currency, model.TopCurrency, model.WinlostDate);
                }
                else
                {
                    this._monitorCacheService.SetTopSelectionByTurnoverEventByCurrencyCache(model.Selection, sportType, model.Currency, model.TopCurrency, model.WinlostDate);
                }
            }
            else
            {
                // by League
                if (model.IsTicket)
                {
                    this._monitorCacheService.SetTopSelectionByTicketEventByLeagueCache(model.Selection, sportType, model.LeagueId, model.WinlostDate);
                }
                else
                {
                    this._monitorCacheService.SetTopSelectionByTurnoverEventByLeagueCache(model.Selection, sportType, model.LeagueId, model.WinlostDate);
                }
            }
        }

        /// <summary>
        /// Updates top leagues and top currencies in the memory cache.
        /// </summary>
        /// <param name="model">MonitoringDetailModel object contains top leagues and top currencies.</param>
        public void SetSelectionByTicket(MonitoringDetailModel model)
        {
            DateTime winlostDate = model.WinlostDate;
            int selection = model.Selection;
            string sportName = model.SportType;
            int sport = this.GetSportTypeIdByName(sportName);
            this._monitorCacheService.SetTopSelectionByTicketCache(selection, sport, winlostDate);
        }

        /// <summary>
        /// Updates top leagues and top currencies in the memory cache.
        /// </summary>
        /// <param name="model">MonitoringDetailModel object that contains top leagues and top currencies.</param>
        public void SetSelectionByTurnover(MonitoringDetailModel model)
        {
            DateTime winlostDate = model.WinlostDate;
            int selection = model.Selection;
            string sportName = model.SportType;
            int sport = this.GetSportTypeIdByName(sportName);
            this._monitorCacheService.SetTopSelectionByTurnoverCache(selection, sport, winlostDate);
        }

        /// <summary>
        /// Sets favorite choices to the cache.
        /// </summary>
        /// <param name="model">MonitoringDetailModel object that contains the favorite choices.</param>
        public void SetCacheFavouriteChoice(MonitoringCacheModel model)
        {
            int sportType = GetSportTypeIdByName(model.SportType);
            if (model.IsTicket)
            {
                this._monitorCacheService.SetTopSelectionByTicketEventCache(model.Selection, sportType, model.WinlostDate);
                if (model.IsBettype)
                {
                    this._monitorCacheService.SetTopSelectionByTicketEventBettypeCache(model.Selection, sportType, model.MatchId, model.WinlostDate);
                    if (model.IsChoice)
                    {
                        this._monitorCacheService.SetTopSelectionByTicketEventBettypeChoiceCache(model.Selection, sportType, model.MatchId, model.Bettype2, model.WinlostDate);
                    }
                }
            }
            else
            {
                this._monitorCacheService.SetTopSelectionByTurnoverEventCache(model.Selection, sportType, model.WinlostDate);
                if (model.IsBettype)
                {
                    this._monitorCacheService.SetTopSelectionByTurnoverBettypeCache(model.Selection, sportType, model.MatchId, model.WinlostDate);
                    if (model.IsChoice)
                    {
                        this._monitorCacheService.SetTopSelectionByTurnoverEventBettypeChoiceCache(model.Selection, sportType, model.MatchId, model.Bettype2, model.WinlostDate);
                    }
                }
            }
        }

        #endregion
    }
}
