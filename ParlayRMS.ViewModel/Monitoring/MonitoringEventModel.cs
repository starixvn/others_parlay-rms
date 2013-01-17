// -----------------------------------------------------------------------
// <copyright file="MonitoringEventModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.Monitoring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MonitoringEventModel : MonitoringDetailModel
    {
        public int CurrencyId { get; set; }

        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        public string MatchName { get; set; }

        public int BetType2 { get; set; }

        public string BetTypeName { get; set; }

        public string TopCurrency { get; set; }

        public string CurrencyName { get; set; }

        public string LeagueName { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }
    }
}