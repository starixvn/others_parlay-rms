// -----------------------------------------------------------------------
// <copyright file="MonitoringSelectedInfo.cs" company="">
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
    public class MonitoringDetailModel
    {
        public MonitoringDetailModel()
        {
            this.WinlostDate = Config.WinlostDate;
        }

        public DateTime WinlostDate { get; set; }

        public int Selection { get; set; }

        public string SportType { get; set; }

        public long MaxTransId { get; set; }
    }
}