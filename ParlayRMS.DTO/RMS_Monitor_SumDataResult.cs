using System;

namespace ParlayRMS.DTO
{
    public class RMS_Monitor_SumDataResult
    {
        public int Events { get; set; }

        public string Status { get; set; }

        public int Tickets { get; set; }

        public decimal Turnover { get; set; }

        public decimal Winloss { get; set; }
    }
}