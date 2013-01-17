namespace ParlayRMS.Models.Systems
{
    using System;
    using System.Globalization;

    public class MaintenanceInfo
    {
        public MaintenanceInfo()
        {
            IsDownTime = false;
            TimeFrom = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            TimeTo = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Checking server is under downtime
        /// </summary>
        public bool IsDownTime { get; set; }

        /// <summary>
        /// Get from time
        /// </summary>
        public string TimeFrom { get; set; }

        /// <summary>
        /// Get to time
        /// </summary>
        public string TimeTo { get; set; }
    }
}