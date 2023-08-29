namespace ParlayRMS.ViewModel.ManageGroup
{
    using System.ComponentModel.DataAnnotations;
    using Utility;

    public class EditGroupModel
    {
        [Required]
        public int GroupId { get; set; }

        [Required]
        [Display(Name = "GroupName", ResourceType = typeof(ViewLang))]
        public string GroupName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ViewLang))]
        public string Description { get; set; }
    }
}
