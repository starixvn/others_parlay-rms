namespace ParlayRMS.ViewModel.ManageMenu
{
    using System.ComponentModel.DataAnnotations;
    using Utility;

    public class EditMenuModel
    {
        [Required]
        public int MenuId { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewLang))]
        public string MenuName { get; set; }

        [Display(Name = "Url", ResourceType = typeof(ViewLang))]
        public string Url { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ViewLang))]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Function", ResourceType = typeof(ViewLang))]
        public int FunctionId { get; set; }

        [Required]
        [Display(Name = "Parent", ResourceType = typeof(ViewLang))]
        public int ParentId { get; set; }

        [Required]
        public string ParentName { get; set; }

        [Required, Range(0, int.MaxValue)]
        [Display(Name = "Order", ResourceType = typeof(ViewLang))]
        public int DisplayOrder { get; set; }
    }
}
