// -----------------------------------------------------------------------
// <copyright file="MonitoringCacheModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.Monitoring
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using ParlayRMS.ViewModel.Shared;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MonitoringCacheModel
    {
        public int BetType { get; set; }

        public DateTime WinlostDate { get; set; }

        public int Selection { get; set; }

        public string SportType { get; set; }

        public bool IsTicket { get; set; }

        public bool IsValid()
        {
            if (Selection == 0)
                return true;

            return false;
        }

        public int Currency { get; set; }

        public string TopCurrency { get; set; }

        public int LeagueId { get; set; }

        public bool isSelectDetail { get; set; }

        public bool isBettype { get; set; }

        public bool isChoice { get; set; }

        public bool isEvent { get; set; }

        public int matchId { get; set; }

        public int bettype2 { get; set; }

        public MonitoringCacheModel()
        {
            this.WinlostDate = Config.WinlostDate;
        }
    }
}