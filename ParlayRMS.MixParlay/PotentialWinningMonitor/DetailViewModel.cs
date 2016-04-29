using System.ComponentModel.DataAnnotations;

namespace ParlayRMS.MixParlay.PotentialWinningMonitor
{
    public class DetailViewModel
    {
        [Required]
        public string Combos { get; set; }

        [Required]
        public string SportTypes { get; set; }

        [Required]
        public long CustId { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}