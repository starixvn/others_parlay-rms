using System;

namespace ParlayRMS.DTO
{
    public class RMS_Monitor_TopSelByTicketResult
    {
        public int All_Sports { get; set; }

        public int Combined_Sports { get; set; }

        public int Other_Sports { get; set; }

        public byte Selection_Type { get; set; }

        public int Soccer_Only { get; set; }
    }
}