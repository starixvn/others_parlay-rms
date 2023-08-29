namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeLevelModel
    {
        [Required]
        public int CustId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required, Range(0, 10)]
        public int Level { get; set; }
    }
}
