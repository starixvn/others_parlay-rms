namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;
    using Utility;

    public class AddMemberModel
    {
        [Required]
        [Display(Name = "Username", ResourceType = typeof(ViewLang))]
        public string UserName { get; set; }

        [Required, Range(0, 10)]
        [Display(Name = "Level", ResourceType = typeof(ViewLang))]
        public int Level { get; set; }
    }
}
