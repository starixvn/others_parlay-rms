using System.ComponentModel.DataAnnotations;

namespace ParlayRMS.ViewModel.ViewIPInfo
{
    public class ParlayBetTypeViewModel
    {
        [Required]
        public long TransId { get; set; }

        [Required]
        public long RefNo { get; set; }

        [Required]
        public int BetType { get; set; }

        public int Hdp1 { get; set; }

        public int Hdp2 { get; set; }

        public string BetTeam { get; set; }

        public string BetCheck { get; set; }
    }
}