namespace ParlayRMS.ViewModel.ManageGroup
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Utility;

    public class CreateGroupPolicyModel
    {
        [Required]
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        [Required]
        public string GroupPolicy { get; set; }
    }
}