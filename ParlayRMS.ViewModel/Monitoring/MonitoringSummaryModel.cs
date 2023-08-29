namespace ParlayRMS.ViewModel.Monitoring
{
    using System;

    public class MonitoringSummaryModel
    {
        public MonitoringSummaryModel()
        {
            All_Sports = true;
            Soccer_Only = true;
            Combined_Sports = true;
            Other_Sports = true;
            WinlostDate = Config.WinlostDate;
        }

        public long MaxTransId { get; set; }

        public DateTime WinlostDate { get; set; }

        public bool All_Sports { get; set; }

        public bool Soccer_Only { get; set; }

        public bool Combined_Sports { get; set; }

        public bool Other_Sports { get; set; }

        public bool[] States
        {
            get
            {
                return new[] { All_Sports, Soccer_Only, Combined_Sports, Other_Sports };
            }
        }
    }
}
