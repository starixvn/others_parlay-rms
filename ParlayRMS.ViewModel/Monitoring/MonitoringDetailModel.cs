namespace ParlayRMS.ViewModel.Monitoring
{
    using System;

    public class MonitoringDetailModel
    {
        public MonitoringDetailModel()
        {
            WinlostDate = Config.WinlostDate;
        }

        public DateTime WinlostDate { get; set; }

        public int Selection { get; set; }

        public string SportType { get; set; }

        public long MaxTransId { get; set; }
    }
}
