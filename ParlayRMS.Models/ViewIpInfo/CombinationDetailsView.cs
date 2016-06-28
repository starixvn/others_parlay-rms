// --------------------------------------------------------------------------
// <copyright file="CombinationDetailsView.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.22">Created</change>
// </history>

namespace ParlayRMS.Models.ViewIpInfo
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Bet combination details view model.
    /// </summary>
    public class CombinationDetailsView
    {
        public DateTime Date { get; set; }

        public List<EventItem> Events { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal WinLoss { get; set; }

        public int Status { get; set; }

        public string StatusString { get; set; }

        /// <summary>
        /// Event item class.
        /// </summary>
        public partial class EventItem
        {
            public EventItem(int homeId, int awayId)
            {
                this.HomeId = homeId;
                this.AwayId = awayId;
            }

            public int HomeId { get; set; }

            public int AwayId { get; set; }
        }
    }
}