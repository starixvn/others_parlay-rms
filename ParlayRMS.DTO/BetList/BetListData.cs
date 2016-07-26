using System.Collections.Generic;

namespace ParlayRMS.DTO.BetList
{
    public class BetListData
    {
        public IEnumerable<RMS_Parlay_TransDetails> Tickets { get; set; }

        public string CurrencyName { get; set; }
    }
}