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

        public decimal BoostOdds { get; set; }

        public bool IsBoostBonus { get; set; }

        public decimal OriginalOdds { get; set; }

        public decimal BoostRate { get; set; }

        public decimal Bba { get; set; }

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
