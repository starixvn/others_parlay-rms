namespace ParlayRMS.ViewModel.ViewIPInfo
{
    using System;

    public class ViewIPStatsOptions
    {
        public DateTime FromDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public long MaxTransId { get; set; }
    }
}
