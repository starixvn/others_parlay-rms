namespace ParlayRMS.Models.ViewIpInfo
{
    using System;
    using System.Collections.Generic;

    public class CombinationDetailsByDateViews
    {
        public string BetTypeName { get; set; }

        public decimal Outstanding { get; set; }

        public Dictionary<DateTime, List<CombinationDetailsView>> ViewDict { get; set; }
    }
}
