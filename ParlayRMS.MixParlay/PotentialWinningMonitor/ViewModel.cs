using System.ComponentModel.DataAnnotations;

namespace ParlayRMS.MixParlay.PotentialWinningMonitor
{
    public class ViewModel
    {
        [Required]
        public string Combos { get; set; }

        [Required]
        public string SportTypes { get; set; }

        [Required]
        public string Levels { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal WinningAmount { get; set; }

        [Required]
        public int SelectTop { get; set; }
    }
}