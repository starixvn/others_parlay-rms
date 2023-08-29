namespace ParlayRMS.ViewModel.ManageUser
{
    using System.ComponentModel.DataAnnotations;
    using Utility;

    public class ResetPasswordModel
    {
        [Required]
        public int UserId { get; set; }

        [Display(Name = "Username", ResourceType = typeof(ViewLang))]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmpassword", ResourceType = typeof(ViewLang))]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Newpassword", ResourceType = typeof(ViewLang))]
        public string NewPassword { get; set; }
    }
}
