namespace ParlayRMS.ViewModel.ManageLeague
{
    using System.ComponentModel.DataAnnotations;

    public class AddLeagueModel
    {
        [Required]
        [Display(Name = "League")]
        public string League { get; set; }

        [Required]
        [Display(Name = "Group")]
        public int Group { get; set; }
    }
}