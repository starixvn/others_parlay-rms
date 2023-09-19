using System.Web.Mvc;

namespace ParlayRMS.ViewModel.Report
{
    public class BetDetailsOptions
    {
        public long TransId { get; set; }

        public long OriginalTransId { get; set; }

        public long RefNo { get; set; }

        public long OriginalRefNo { get; set; }

        [AllowHtml]
        public string Username { get; set; }
    }
}
