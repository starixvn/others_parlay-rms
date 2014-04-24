// --------------------------------------------------------------------------
// <copyright file="CombinationDetailsView.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.22">Created</change>
// </history>

namespace ParlayRMS.Models.ViewIpInfo
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.ViewIPInfo;

    /// <summary>
    /// Bet combination details view model.
    /// </summary>
    public class CombinationDetailsView
    {
        public DateTime Date { get; set; }

        public List<EventItem> Events { get; set; }

        public double Odds { get; set; }

        public double Stake { get; set; }

        public double WinLoss { get; set; }

        public int Status { get; set; }

        public string StatusString { get; set; }

        public partial class EventItem
        {
            public int HomeId { get; set; }

            public int AwayId { get; set; }

            public EventItem(int homeId, int awayId)
            {
                this.HomeId = homeId;
                this.AwayId = awayId;
            }
        }
    }
}