// --------------------------------------------------------------------------
// <copyright file="MonitoringSummaryModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.Monitoring
{
    using System;

    /// <summary>
    /// Monitoring Summary Model.
    /// </summary>
    public class MonitoringSummaryModel
    {
        public MonitoringSummaryModel()
        {
            this.All_Sports = true;
            this.Soccer_Only = true;
            this.Combined_Sports = true;
            this.Other_Sports = true;
            this.WinlostDate = Config.WinlostDate;
        }

        public long MaxTransId { get; set; }

        public DateTime WinlostDate { get; set; }

        public bool All_Sports { get; set; }

        public bool Soccer_Only { get; set; }

        public bool Combined_Sports { get; set; }

        public bool Other_Sports { get; set; }

        public bool[] States
        {
            get
            {
                return new bool[] { this.All_Sports, this.Soccer_Only, this.Combined_Sports, this.Other_Sports };
            }
        }
    }
}