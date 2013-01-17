// -----------------------------------------------------------------------
// <copyright file="CacheController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ParlayRMS.ViewModel.Monitoring;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Implement;
    using ParlayRMS.Services.Implement.Charts;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CacheController : BaseController
    {
        private readonly ChartsCacheService monitorCacheService = new ChartsCacheService();

        #region Chart

        /// <summary>
        /// Update data cache for Summary Data, Top Selection By Ticket and Top Selection By Turnover
        /// </summary>
        /// <param name="model"></param>
        public void SetTopSelection(MonitoringCacheModel model)
        {
            int sportType = GetSportTypeIdByName(model.SportType);
            this.monitorCacheService.SetCache(model.BetType, model.WinlostDate, model.IsTicket, model.Selection, sportType);
            if (model.isSelectDetail)
            {
                this.SetTopEvent(model);
            }

            if (model.isEvent)
            {
                this.SetCacheFavouriteChoice(model);
            }
        }

        public void SetTopEvent(MonitoringCacheModel model)
        {
            int sportType = GetSportTypeIdByName(model.SportType);
            if (model.Currency != -1)
            {
                // by Currency
                if (model.IsTicket)
                {
                    this.monitorCacheService.SetTopSelectionByTicketEventByCurrencyCache(model.Selection, sportType, model.Currency, model.TopCurrency, model.WinlostDate);
                }
                else
                {
                    this.monitorCacheService.SetTopSelectionByTurnoverEventByCurrencyCache(model.Selection, sportType, model.Currency, model.TopCurrency, model.WinlostDate);
                }
            }
            else
            {
                // by League
                if (model.IsTicket)
                {
                    this.monitorCacheService.SetTopSelectionByTicketEventByLeagueCache(model.Selection, sportType, model.LeagueId, model.WinlostDate);
                }
                else
                {
                    this.monitorCacheService.SetTopSelectionByTurnoverEventByLeagueCache(model.Selection, sportType, model.LeagueId, model.WinlostDate);
                }
            }
        }

        /// <summary>
        /// Upate data cache for Top Currency By Ticket and Top League By Ticket
        /// </summary>
        /// <param name="model"></param>
        public void SetSelectionByTicket(MonitoringDetailModel model)
        {
            DateTime winlostDate = model.WinlostDate;
            int selection = model.Selection;
            string sportName = model.SportType;
            int sport = this.GetSportTypeIdByName(sportName);
            this.monitorCacheService.SetTopSelectionByTicketCache(selection, sport, winlostDate);
        }

        /// <summary>
        /// Upate data cache for Top Currency By Turnover and Top League By Turnover
        /// </summary>
        /// <param name="model"></param>
        public void SetSelectionByTurnover(MonitoringDetailModel model)
        {
            DateTime winlostDate = model.WinlostDate;
            int selection = model.Selection;
            string sportName = model.SportType;
            int sport = this.GetSportTypeIdByName(sportName);
            this.monitorCacheService.SetTopSelectionByTurnoverCache(selection, sport, winlostDate);
        }

        public void SetCacheFavouriteChoice(MonitoringCacheModel model)
        {
            int sportType = GetSportTypeIdByName(model.SportType);
            if (model.IsTicket)
            {
                this.monitorCacheService.SetTopSelectionByTicketEventCache(model.Selection, sportType, model.WinlostDate);
                if (model.isBettype)
                {
                    this.monitorCacheService.SetTopSelectionByTicketEventBettypeCache(model.Selection, sportType, model.matchId, model.WinlostDate);
                    if (model.isChoice)
                    {
                        this.monitorCacheService.SetTopSelectionByTicketEventBettypeChoiceCache(model.Selection, sportType, model.matchId, model.bettype2, model.WinlostDate);
                    }
                }
            }
            else
            {
                this.monitorCacheService.SetTopSelectionByTurnoverEventCache(model.Selection, sportType, model.WinlostDate);
                if (model.isBettype)
                {
                    this.monitorCacheService.SetTopSelectionByTurnoverBettypeCache(model.Selection, sportType, model.matchId, model.WinlostDate);
                    if (model.isChoice)
                    {
                        this.monitorCacheService.SetTopSelectionByTurnoverEventBettypeChoiceCache(model.Selection, sportType, model.matchId, model.bettype2, model.WinlostDate);
                    }
                }
            }
        }

        #endregion
    }
}
