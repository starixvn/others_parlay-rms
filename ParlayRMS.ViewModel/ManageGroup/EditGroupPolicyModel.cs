namespace ParlayRMS.ViewModel.ManageGroup
{
    using System.ComponentModel.DataAnnotations;

    public class EditGroupPolicyModel
    {
        [Required]
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        [Required]
        public string GroupPolicy { get; set; }
    }
}
