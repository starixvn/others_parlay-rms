namespace ParlayRMS.Models.ViewIpInfo
{
    using System.Collections.Generic;

    public class CombinationDetailsViews
    {
        public decimal Outstanding { get; set; }

        public IEnumerable<CombinationDetailsView> ViewList { get; set; }
    }
}
