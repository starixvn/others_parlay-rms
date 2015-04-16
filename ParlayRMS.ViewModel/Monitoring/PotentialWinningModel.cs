// --------------------------------------------------------------------------
// <copyright file="PotentialWinningModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.Monitoring
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Web.Mvc.DataAnnotations;

    /// <summary>
    /// Data Model.
    /// </summary>
    public class PotentialWinningModel
    {
        public string Combos { get; set; }

        public string SportsTypes { get; set; }
        
        public string Levels { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal WinningAmount { get; set; }        

        [Required]
        public int SelectTop { get; set; }
    }
}