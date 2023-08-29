namespace ParlayRMS.ViewModel.ManageAgent
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Utility;

    public class AddAgentModel
    {
        [Required]
        [Display(Name = "Username", ResourceType = typeof(ViewLang))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [AllowHtml]
        public string UserName { get; set; }
    }
}
