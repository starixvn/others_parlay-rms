using ParlayRMS.DTO.ViewIPInfo;
using System;

namespace ParlayRMS.Models.ViewIpInfo
{
    public class DrillDownViewModel
    {
        public DrillDownViewModel(RMS_Rpt_Parlay_ViewIPInfoByUser model)
        {
            RefNo = model.RefNo;
            TransDate = model.TransDate;
            Stake = model.Stake;
            Winlost = model.Winlost;
            Status = model.Status;
            Odds = model.Odds;
            Choice = string.Empty;
            BetType = model.BetType;
        }

        public long RefNo { get; set; }

        public DateTime TransDate { get; set; }

        public string Choice { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal Winlost { get; set; }

        public string Status { get; set; }

        public int BetType { get; set; }

        public int Combo { get; set; }
    }
}
