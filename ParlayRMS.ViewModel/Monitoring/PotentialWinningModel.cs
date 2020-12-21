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
    }
}