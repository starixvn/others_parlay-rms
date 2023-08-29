using System.Collections.Generic;

namespace ParlayRMS.ViewModel.Report
{
    public class BetDetails
    {
        public string TicketDetail { get; set; }

        public string TicketCompare { get; set; }

        public IList<string> ComboTypes { get; set; }
    }
}
