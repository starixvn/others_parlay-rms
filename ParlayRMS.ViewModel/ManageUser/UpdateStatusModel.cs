namespace ParlayRMS.ViewModel.ManageUser
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateStatusModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
