namespace ParlayRMS.ViewModel.ManageGroup
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Utility;

    public class CreateGroupModel
    {
        [Required]
        [Display(Name = "GroupName", ResourceType = typeof(ViewLang))]
        public string GroupName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ViewLang))]
        public string Description { get; set; }
    }
}