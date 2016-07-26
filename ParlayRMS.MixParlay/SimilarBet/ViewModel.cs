using System.ComponentModel.DataAnnotations;

namespace ParlayRMS.MixParlay.SimilarBet
{
    public class ViewModel
    {
        [Required]
        public long TransId { get; set; }
    }
}