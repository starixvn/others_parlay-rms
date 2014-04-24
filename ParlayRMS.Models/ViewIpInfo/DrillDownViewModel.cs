// --------------------------------------------------------------------------
// <copyright file="DrillDownViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.22">Created</change>
// </history>

namespace ParlayRMS.Models.ViewIpInfo
{
    using System;
    using ParlayRMS.DTO.ViewIPInfo;

    /// <summary>
    /// Model for ViewIPInfo drill down report.
    /// </summary>
    public class DrillDownViewModel
    {
        public long RefNo { get; set; }

        public DateTime TransDate { get; set; }

        public string Choice { get; set; }

        public double Odds { get; set; }

        public int Stake { get; set; }

        public int Winlost { get; set; }

        public string Status { get; set; }

        public int BetType { get; set; }

        public int Combo { get; set; }

        public DrillDownViewModel(RMS_Rpt_Parlay_ViewIPInfoByUser model)
        {
            this.RefNo = model.RefNo;
            this.TransDate = model.TransDate;
            this.Stake = model.Stake;
            this.Winlost = model.Winlost;
            this.Status = model.Status;
            this.Odds = model.Odds;
            this.Choice = string.Empty;
            this.BetType = model.BetType;
            this.Combo = model.Combo;
            //this.BetType = 29;
            //this.Combo = 4;
        }
    }
}