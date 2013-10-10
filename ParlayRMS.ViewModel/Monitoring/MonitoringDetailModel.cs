// --------------------------------------------------------------------------
// <copyright file="MonitoringDetailModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.Monitoring
{
    using System;

    /// <summary>
    /// Monitoring Detail Model.
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