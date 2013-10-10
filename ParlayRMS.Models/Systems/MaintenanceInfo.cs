// --------------------------------------------------------------------------
// <copyright file="MaintenanceInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Systems
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Maintenance Information.
    /// </summary>
    public class MaintenanceInfo
    {
        public MaintenanceInfo()
        {
            IsDownTime = false;
            TimeFrom = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            TimeTo = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        public bool IsDownTime { get; set; }

        public string TimeFrom { get; set; }

        public string TimeTo { get; set; }
    }
}