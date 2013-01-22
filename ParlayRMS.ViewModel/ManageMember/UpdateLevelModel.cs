namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateLevelModel
    {
        [Required]
        public int CustId { get; set; }

        [Required, Range(0, 10)]
        public int Level { get; set; }
    }
}