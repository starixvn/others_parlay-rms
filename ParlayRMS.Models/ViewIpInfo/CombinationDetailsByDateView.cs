namespace ParlayRMS.Models.ViewIpInfo
{
    using System;
    using System.Collections.Generic;

    public class CombinationDetailsByDateView
    {
        public DateTime Date { get; set; }

        public List<CombinationDetailsView> ViewItem { get; set; }
    }
}
