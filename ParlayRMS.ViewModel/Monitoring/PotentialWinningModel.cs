using System;
using System.ComponentModel.DataAnnotations;

namespace ParlayRMS.ViewModel.Monitoring
{
    public class PotentialWinningModel
    {
        public string Combos { get; set; }

        public string SportsTypes { get; set; }

        public string ParlayTypes { get; set; }

        public string Levels { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal WinningAmount { get; set; }

        [Required]
        public int SelectTop { get; set; }

        public bool IsCornerParlay { get; set; }

        public byte FilterBy { get; set; }

        public int? AgentId { get; set; }

        public Int64? CustomerClass { get; set; }

        public string Company { get; set; }

        public string UserName { get; set; }
    }
}