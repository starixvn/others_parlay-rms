namespace ParlayRMS.DTO.ViewIPInfo
{
    using System.Collections.Generic;

    public class RMS_SystemParlay_CombinationDetails
    {
        public decimal Outstanding { get; set; }

        public IEnumerable<RMS_SystemParlay_CombinationDetails_Combo> Combo { get; set; }

        public IEnumerable<RMS_SystemParlay_CombinationDetails_ComboRef> ComboRef { get; set; }
    }
}
