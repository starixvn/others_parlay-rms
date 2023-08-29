namespace ParlayRMS.Models.ViewIpInfo
{
    using System;
    using System.Collections.Generic;

    public class CombinationDetailsView
    {
        public DateTime Date { get; set; }

        public List<EventItem> Events { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal WinLoss { get; set; }

        public int Status { get; set; }

        public string StatusString { get; set; }

        public class EventItem
        {
            public EventItem(int homeId, int awayId)
            {
                HomeId = homeId;
                AwayId = awayId;
            }

            public int HomeId { get; set; }

            public int AwayId { get; set; }
        }
    }
}
