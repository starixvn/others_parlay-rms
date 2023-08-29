namespace ParlayRMS.DTO.ViewIPInfo
{
    using System;

    public class RMS_SystemParlay_CombinationDetails_Combo
    {
        public DateTime WinLostDate { get; set; }

        public long TransID { get; set; }

        public int Serial { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public decimal Modds { get; set; }

        public decimal Stake { get; set; }

        public decimal Winlost { get; set; }
    }
}
