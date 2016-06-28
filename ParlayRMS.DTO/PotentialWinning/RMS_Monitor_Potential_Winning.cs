// --------------------------------------------------------------------------
// <copyright file="RMS_Monitor_Potential_Winning.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.PotentialWinning
{
    using System;

    /// <summary>
    /// Monitor Potential Winning List model.
    /// </summary>
    public class RMS_Monitor_Potential_Winning
    {
        public DateTime TransDate { get; set; }

        public string TransDateString { get; set; }

        public long CustId { get; set; }        

        public string UserName { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal WinningAmount { get; set; }

        public decimal CompanyLoseAmount { get; set; }

        public int TotalTicket { get; set; }        

        public int Level { get; set; }        
    }
}